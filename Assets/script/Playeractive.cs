using UnityEngine;
using UnityEngine.SceneManagement;

public class Playeractive : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public static Playeractive instance;

    public GameObject attackEffectPrefab;
    public Transform attackSpawnPoint;
    public float attackSpawnDistance = 2.7f;

    private bool canAttack = true;

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
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject startPoint = GameObject.Find("StartPoint");
        if (startPoint != null)
        {
            transform.position = startPoint.transform.position;
        }
    }

    void Update()
    {
        bool isTalking = (TalkManager.instance != null && TalkManager.isTalking);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(
            mousePos.x - transform.position.x,
            mousePos.y - transform.position.y
        );
        direction.Normalize();

        if (isTalking || !canAttack)
        {
            inputVec = Vector2.zero;
        }
        else
        {
            inputVec.x = Input.GetAxisRaw("Horizontal");
            inputVec.y = Input.GetAxisRaw("Vertical");

            spriter.flipX = (mousePos.x < transform.position.x);
            if (attackSpawnPoint != null)
            {
                attackSpawnPoint.localPosition = direction * attackSpawnDistance;
            }
        }

        if (Input.GetButtonDown("Fire1") && canAttack)
        {
            canAttack = false;
            anim.SetTrigger("Attack");

            SpawnAttackEffect(direction);

            spriter.flipX = (mousePos.x < transform.position.x);
        }
    }

    public void ResetAttackCooldown()
    {
        canAttack = true;
    }

    void SpawnAttackEffect(Vector2 direction)
    {
        if (attackEffectPrefab != null && attackSpawnPoint != null)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Instantiate(attackEffectPrefab, attackSpawnPoint.position, rotation);
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