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

    public float AttackSpeed = 1.5f;
    public float attackDelay = 0.4f;
    public float baseEffectLifeTime = 0.5f;

    [Header("Mana Settings")]
    public float MaxMana = 100;
    public float CurrentMana;
    public float manaRegenRate = 1.0f;

    public float Exp = 0;
    public int Level = 0;
    public int Money = 0;

    public GameObject attackEffectPrefab;
    public Transform attackSpawnPoint;
    public float attackSpawnDistance = 2.7f;

    private float curTime = 0.0f;

    public bool isAttacking { get; private set; } = false;

    // [추가] 대화 중이라 플레이어가 잠겼는지 확인하는 변수
    private bool isDialogueLocked = false;

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

        // [수정] 기존 조건에 'isDialogueLocked' 추가
        // 대화 모드(GenericNPC)로 잠겨있으면 이동 불가
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

        // [수정] 대화 중 잠겨있으면(isDialogueLocked) 공격도 못하게 조건 추가
        if (Input.GetButtonDown("Fire1") && curTime >= (1.0f / AttackSpeed) && !isAttacking && !isDialogueLocked)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    // [추가] NPC가 호출할 함수: 플레이어를 안 보이게 하고 움직임도 막음
    public void SetDialogueState(bool isActive)
    {
        isDialogueLocked = isActive; // 이동 및 공격 잠금/해제

        if (spriter != null)
        {
            // 대화 중(isActive == true)이면 스프라이트를 끕니다 -> 투명해짐
            // 대화 끝(isActive == false)이면 스프라이트를 켭니다 -> 다시 보임
            spriter.enabled = !isActive;
        }

        // 잠기는 순간 미끄러지지 않게 속도 0으로 고정
        if (isActive)
        {
            inputVec = Vector2.zero;
            if (rigid != null) rigid.linearVelocity = Vector2.zero;
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