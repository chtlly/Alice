using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Playeractive : MonoBehaviour
{
    public static Playeractive instance;

    [Header("Player Stats")]
    public float MaxHp = 100000;
    public float CurrentHp;
    public float Attack = 10;
    public float Defense = 10;

    // 공격 설정
    public float AttackSpeed = 1.5f;
    public float attackDelay = 0.4f;
    public float baseEffectLifeTime = 0.5f;

    [Header("Mana Settings")]
    public float MaxMana = 100;
    public float CurrentMana;
    // [추가] 마나 자연 회복량 (초당 1)
    public float manaRegenRate = 1.0f;

    // 기타 스탯
    public float Exp = 0;
    public int Level = 0;
    public int Money = 0;

    public GameObject attackEffectPrefab;
    public Transform attackSpawnPoint;
    public float attackSpawnDistance = 2.7f;

    private float curTime = 0.0f;

    public bool isAttacking { get; private set; } = false;

    public Vector2 inputVec;
    public float speed;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
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
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (rigid != null) rigid.linearVelocity = Vector2.zero;
        GameObject startPoint = GameObject.Find("StartPoint");
        if (startPoint != null) transform.position = startPoint.transform.position;
    }

    private void OnDestroy() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void Update()
    {
        // --- [추가됨] 마나 자연 회복 로직 ---
        if (CurrentMana < MaxMana)
        {
            // 초당 1씩 회복 (Time.deltaTime을 곱해야 프레임 상관없이 1초당 1씩 오름)
            CurrentMana += manaRegenRate * Time.deltaTime;

            // 최대치 넘지 않게 고정
            if (CurrentMana > MaxMana) CurrentMana = MaxMana;

            // UI 갱신 (부드럽게 차오르는 거 보여주기 위해 매 프레임 호출)
            if (UIManager.instance != null)
            {
                UIManager.instance.UpdateMana(CurrentMana, MaxMana);
            }
        }
        // ----------------------------------

        bool isTalking = (TalkManager.instance != null && TalkManager.isTalking);
        curTime += Time.deltaTime;

        if (isTalking || isAttacking)
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

        if (Input.GetButtonDown("Fire1") && curTime >= (1.0f / AttackSpeed) && !isAttacking)
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
        if (CurrentHp <= 0) gameObject.SetActive(false);
        return finalDamage;
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

    void FixedUpdate() { Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; rigid.MovePosition(rigid.position + nextVec); }
    private void LateUpdate() { anim.SetFloat("Speed", inputVec.magnitude); }
    public void LookAt(Vector3 targetPos) { spriter.flipX = targetPos.x < transform.position.x; }
}