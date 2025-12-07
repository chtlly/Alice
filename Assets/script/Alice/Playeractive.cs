using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Playeractive : MonoBehaviour
{
    public static Playeractive instance;

    [Header("Player Stats")]
    public float MaxHp = 100000;
    public float CurrentHp;
    public float Attack = 10;
    public float Defense = 10;

    public float AttackSpeed = 1.5f;
    public float maxAttackSpeed = 3.0f;
    public float attackDelay = 0.4f;
    public float baseEffectLifeTime = 0.5f;

    [Header("Mana Settings")]
    public float MaxMana = 100;
    public float CurrentMana;
    public float manaRegenRate = 1.0f;
    public float maxManaRegen = 5.0f;

    [Header("Level Settings")]
    public int Level = 1;
    public float Exp = 0;
    public float MaxExp = 5;
    public int Money = 0;

    public GameObject attackEffectPrefab;
    public Transform attackSpawnPoint;
    public float attackSpawnDistance = 2.7f;

    public GameObject damageTextPrefab;

    private float curTime = 0.0f;
    public bool isAttacking { get; private set; } = false;
    private bool isDialogueLocked = false;

    public Vector2 inputVec;
    public float speed;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        SceneManager.sceneLoaded += OnSceneLoaded;

        CurrentHp = MaxHp;
        CurrentMana = MaxMana;
    }

    void Start()
    {
        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateHP(CurrentHp, MaxHp);
            UIManager.instance.UpdateMana(CurrentMana, MaxMana);
            UIManager.instance.UpdateExp(Exp, MaxExp, Level);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (rigid != null) rigid.linearVelocity = Vector2.zero;
        GameObject startPoint = GameObject.Find("StartPoint");
        if (startPoint != null) transform.position = startPoint.transform.position;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (CurrentMana < MaxMana)
        {
            CurrentMana += manaRegenRate * Time.deltaTime;
            if (CurrentMana > MaxMana) CurrentMana = MaxMana;

            if (UIManager.instance != null)
            {
                UIManager.instance.UpdateMana(CurrentMana, MaxMana);
            }
        }

        bool isTalking = (TalkManager.instance != null && TalkManager.isTalking);
        curTime += Time.deltaTime;

        if (isTalking || isAttacking || isDialogueLocked)
        {
            inputVec = Vector2.zero;
        }
        else
        {
            inputVec.x = Input.GetAxisRaw("Horizontal");
            inputVec.y = Input.GetAxisRaw("Vertical");

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
            Vector2 myPos2D = new Vector2(transform.position.x, transform.position.y);
            Vector2 mouseDir = (mousePos2D - myPos2D).normalized;

            if (inputVec.x != 0) spriter.flipX = inputVec.x < 0;
            if (attackSpawnPoint != null) attackSpawnPoint.localPosition = mouseDir * attackSpawnDistance;
        }

        if (Input.GetButtonDown("Fire1") && curTime >= (1.0f / AttackSpeed) && !isAttacking && !isDialogueLocked)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        inputVec = Vector2.zero;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
        spriter.flipX = (mousePos2D.x < transform.position.x);

        anim.SetTrigger("Attack");

        Vector2 myPos2D = new Vector2(transform.position.x, transform.position.y);
        Vector2 mouseDir = (mousePos2D - myPos2D).normalized;
        SpawnAttackEffect(mouseDir);

        curTime = 0.0f;

        float dynamicDelay = attackDelay / AttackSpeed;
        float attackInterval = 1.0f / AttackSpeed;
        if (dynamicDelay > attackInterval) dynamicDelay = attackInterval;

        yield return new WaitForSeconds(dynamicDelay);

        isAttacking = false;
    }

    public void SetDialogueState(bool isActive)
    {
        isDialogueLocked = isActive;
        if (spriter != null) spriter.enabled = !isActive;
        if (isActive)
        {
            inputVec = Vector2.zero;
            if (rigid != null) rigid.linearVelocity = Vector2.zero;
        }
    }

    public void SetGlobalDelay(float duration)
    {
        StopCoroutine("GlobalDelayRoutine");
        StartCoroutine(GlobalDelayRoutine(duration));
    }

    IEnumerator GlobalDelayRoutine(float duration)
    {
        isAttacking = true;
        inputVec = Vector2.zero;
        yield return new WaitForSeconds(duration);
        isAttacking = false;
    }

    public float TakeDamage(float rawDamage)
    {
        float damageReduction = (rawDamage * Defense) / (100f + Defense);
        float finalDamage = rawDamage - damageReduction;
        if (finalDamage < 1f) finalDamage = 1f;
        finalDamage = Mathf.Floor(finalDamage);
        CurrentHp -= finalDamage;

        if (UIManager.instance != null) UIManager.instance.UpdateHP(CurrentHp, MaxHp);

        if (CurrentHp <= 0)
        {
            CurrentHp = MaxHp;
            CurrentMana = MaxMana;
            isAttacking = false;
            isDialogueLocked = false;
            if (UIManager.instance != null)
            {
                UIManager.instance.UpdateHP(CurrentHp, MaxHp);
                UIManager.instance.UpdateMana(CurrentMana, MaxMana);
            }
            SceneManager.LoadScene("Start_0");
        }
        return finalDamage;
    }
    public bool BuyItem(Item item)
    {
        if (Money < item.price)
        {
            Debug.Log("돈이 부족합니다");
            return false;
        }
        else
        {
            if (QuickSlotController.instance.AddItem(item))
            {
                Money -= item.price;
                Debug.Log($"구매 {item.itemName}. 남은 돈: {Money}");
                return true;
            }
            else
            {
                Debug.Log("슬롯을 확인해 주세요");
                return false;
            }
        }
    }

    public void Heal()
    {
        CurrentHp += MaxHp * 0.3f;
        if (CurrentHp > MaxHp) CurrentHp = MaxHp;
        if (UIManager.instance != null) UIManager.instance.UpdateHP(CurrentHp, MaxHp);
    }
    public void Heal(float amount)
    {
        CurrentHp += amount;
        if (CurrentHp > MaxHp) CurrentHp = MaxHp;
        if (UIManager.instance != null) UIManager.instance.UpdateHP(CurrentHp, MaxHp);
    }

    public bool UseMana(float amount)
    {
        if (CurrentMana >= amount)
        {
            CurrentMana -= amount;
            if (UIManager.instance != null) UIManager.instance.UpdateMana(CurrentMana, MaxMana);
            return true;
        }
        return false;
    }

    public void GainExp(float amount)
    {
        Exp += amount;

        while (Exp >= MaxExp)
        {
            Exp -= MaxExp;
            LevelUp();
        }

        if (UIManager.instance != null) UIManager.instance.UpdateExp(Exp, MaxExp, Level);
    }

    void LevelUp()
    {
        Level++;
        MaxExp = MaxExp * 1.2f;

        MaxHp += 100;
        CurrentHp = MaxHp;
        MaxMana += 10;
        CurrentMana = MaxMana;

        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateHP(CurrentHp, MaxHp);
            UIManager.instance.UpdateMana(CurrentMana, MaxMana);
        }

        List<int> availableStats = new List<int>();

        availableStats.Add(0);
        availableStats.Add(1);

        if (manaRegenRate < maxManaRegen)
        {
            availableStats.Add(2);
        }

        if (AttackSpeed < maxAttackSpeed)
        {
            availableStats.Add(3);
        }

        int randIndex = Random.Range(0, availableStats.Count);
        int selectedStat = availableStats[randIndex];

        string bonusStatText = "";

        switch (selectedStat)
        {
            case 0:
                Attack += 2;
                bonusStatText = "ATK +2";
                break;
            case 1:
                Defense += 1;
                bonusStatText = "DEF +1";
                break;
            case 2:
                manaRegenRate += 0.5f;
                if (manaRegenRate > maxManaRegen) manaRegenRate = maxManaRegen;
                bonusStatText = "Mana Regen +0.5";
                break;
            case 3:
                AttackSpeed += 0.5f;
                if (AttackSpeed > maxAttackSpeed) AttackSpeed = maxAttackSpeed;
                bonusStatText = "ASP +0.5";
                break;
        }

        StartCoroutine(ShowLevelUpText(bonusStatText));
    }

    IEnumerator ShowLevelUpText(string bonusText)
    {
        if (damageTextPrefab != null)
        {
            Vector3 textPos = transform.position + new Vector3(0, 2.0f, -5.0f);

            GameObject levelUpObj = Instantiate(damageTextPrefab, textPos, Quaternion.identity);
            levelUpObj.GetComponent<FloatingText>().SetText("Level Up!", Color.yellow);

            yield return new WaitForSeconds(0.5f);

            GameObject statObj = Instantiate(damageTextPrefab, textPos, Quaternion.identity);
            statObj.GetComponent<FloatingText>().SetText(bonusText, Color.cyan);
        }
    }

    void SpawnAttackEffect(Vector2 direction)
    {
        if (attackEffectPrefab != null)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject obj = Instantiate(attackEffectPrefab, attackSpawnPoint.position, Quaternion.Euler(0, 0, angle));
            PlayerAttack attackScript = obj.GetComponent<PlayerAttack>();
            if (attackScript != null)
            {
                float dynamicLifeTime = baseEffectLifeTime / AttackSpeed;
                attackScript.InitAttack(Attack, dynamicLifeTime);
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude);
    }

    public void LookAt(Vector3 targetPos)
    {
        spriter.flipX = targetPos.x < transform.position.x;
    }
}