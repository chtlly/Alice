using UnityEngine;
using UnityEngine.SceneManagement; // 씬 이벤트를 사용하기 위해 필요합니다.

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    public static PlayerMovement instance;

    // ⭐ 씬 로드 이벤트 구독 ⭐
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // ⭐ 씬 로드 이벤트 구독 해제 (메모리 누수 방지) ⭐
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ⭐ 새 씬이 로드될 때마다 호출되는 함수 ⭐
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 플레이어의 위치를 새 씬의 중앙 (0, 0, 0)으로 재설정합니다.
        // (실제 게임에서는 씬의 특정 Spawn Point로 설정하는 것이 일반적입니다.)
        transform.position = Vector3.zero;

        // Rigidbody를 깨워서 혹시 모를 움직임 버그를 방지합니다.
        if (rb != null)
        {
            rb.WakeUp();
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // ... (Update 및 FixedUpdate 함수는 그대로 유지) ...
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = moveDirection * moveSpeed;
        }
    }
}