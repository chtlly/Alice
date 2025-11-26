using UnityEngine;
using UnityEngine.SceneManagement;

public class Playeractive : MonoBehaviour
{
    public static Playeractive instance;

    [Header("Player Stats")]
    public float MaxHp = 100000;
    public float CurrentHp;
    public float Attack = 10;
    public float Defense = 10;

    public float Mana = 0;
    public float Exp = 0;
    public int Level = 0;
    public int Money = 0;

    public GameObject attackEffectPrefab;
    public Transform attackSpawnPoint;
    public float attackSpawnDistance = 2.7f;

    // [복구됨] 공격 쿨타임 제어 변수
    private bool canAttack = true;

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
    }

    void Start()
    {
        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateHP(CurrentHp, MaxHp);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (rigid != null) rigid.linearVelocity = Vector2.zero;

        GameObject startPoint = GameObject.Find("StartPoint");
        if (startPoint != null)
        {
            transform.position = startPoint.transform.position;
            Debug.Log($"<color=green>이동 완료!</color>");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        bool isTalking = (TalkManager.instance != null && TalkManager.isTalking);

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
        Vector2 myPos2D = new Vector2(transform.position.x, transform.position.y);
        Vector2 mouseDir = (mousePos2D - myPos2D).normalized;

        if (isTalking || !canAttack) // 공격 중엔 이동 불가 (원하면 !canAttack 제거)
        {
            inputVec = Vector2.zero;
        }
        else
        {
            inputVec.x = Input.GetAxisRaw("Horizontal");
            inputVec.y = Input.GetAxisRaw("Vertical");

            if (inputVec.x != 0) spriter.flipX = inputVec.x < 0;
            if (attackSpawnPoint != null) attackSpawnPoint.localPosition = mouseDir * attackSpawnDistance;
        }

        // [복구됨] 공격 가능할 때만(canAttack) 실행
        if (Input.GetButtonDown("Fire1") && canAttack)
        {
            canAttack = false; // 공격 시작 -> 문 잠금
            spriter.flipX = (mousePos2D.x < transform.position.x);
            anim.SetTrigger("Attack");
            SpawnAttackEffect(mouseDir);
        }
    }

    public float TakeDamage(float rawDamage)
    {
        float damageReduction = (rawDamage * Defense) / (100f + Defense);
        float finalDamage = rawDamage - damageReduction;

        if (finalDamage < 1f) finalDamage = 1f;
        finalDamage = Mathf.Floor(finalDamage);

        CurrentHp -= finalDamage;

        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateHP(CurrentHp, MaxHp);
        }

        if (CurrentHp <= 0)
        {
            gameObject.SetActive(false);
        }

        return finalDamage;
    }

    public bool UseMana(float amount)
    {
        if (Mana >= amount)
        {
            Mana -= amount;
            return true;
        }
        return false;
    }

    // [복구됨] 공격 이펙트가 사라질 때 호출되어 다시 공격 가능하게 함
    public void ResetAttackCooldown() => canAttack = true;

    void SpawnAttackEffect(Vector2 direction)
    {
        if (attackEffectPrefab != null)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Instantiate(attackEffectPrefab, attackSpawnPoint.position, Quaternion.Euler(0, 0, angle));
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
}