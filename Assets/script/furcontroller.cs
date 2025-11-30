using UnityEngine;

public class furcontroller : MonoBehaviour

{
    [Header("이동 및 추적")]
    public float moveSpeed = 3f;

    [Header("공격 설정")]
    public float attackRange = 5f; // 공격 사거리 (이 안으로 들어오면 멈춤)
    public float attackCooldown = 2f; // 2초마다 공격
    private float lastAttackTime = 0f; // 마지막으로 공격한 시간 기억

    [Header("Projectile Settings")]
    public GameObject projectilePrefab; // 투사체 프리팹
    public Transform firePoint;         // 발사 위치 (입이나 손)

    // === 내부 컴포넌트 ===
    private Transform playerTransform;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // 게임 시작 시 플레이어를 찾습니다.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        // 플레이어가 게임에 없다면(죽거나 삭제됨) 아무것도 하지 않음
        if (playerTransform == null)
        {
            // (옵션) 혹시 플레이어가 나중에 생성되는 게임이라면 여기서 다시 찾을 수도 있음
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTransform = player.transform;
            return;
        }

        // 1. 거리 계산 (무조건 계산)
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.Normalize();

        // 2. 행동 결정 (감지 범위 체크 없이 바로 사거리만 체크)
        // 공격 사거리보다 멀리 있다면? -> 무조건 추적!
        if (distanceToPlayer > attackRange)
        {
            movement = directionToPlayer; // 이동 방향 설정
            // animator.SetBool("IsChasing", true); // (필요하다면) 걷는 애니메이션 유지
        }
        // 공격 사거리 안으로 들어왔다면? -> 멈춰서 공격!
        else
        {
            movement = Vector2.zero; // 정지
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                // 공격 조건 만족!
                lastAttackTime = Time.time; // 방금 공격했으니 현재 시간을 기록
                animator.SetTrigger("Attack"); // 공격 애니메이션 실행
            }
        }

        // 3. 이미지 좌우 반전 (항상 플레이어 쪽을 바라봄)
        FlipSprite(directionToPlayer.x);
    }

    void FixedUpdate()
    {
        // 이동 적용
        rb.linearVelocity = movement * moveSpeed;
    }

    void FlipSprite(float directionX)
    {
        if (spriteRenderer == null) return;

        // 몬스터 기본 이미지가 왼쪽을 보고 있다고 가정 (사용자분 파일 기준)
        if (directionX > 0 && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true; // 오른쪽 볼 때 뒤집기
        }
        else if (directionX < 0 && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false; // 왼쪽 볼 때 원래대로
        }
    }
    public void FireProjectile()
    {
        if (projectilePrefab == null || firePoint == null || playerTransform == null) return;

        // 1. 발사 각도 계산 (플레이어 위치 - 발사 위치)
        Vector3 direction = playerTransform.position - firePoint.position;

        // 2. 각도를 쿼터니언(회전값)으로 변환
        // (오른쪽을 0도로 기준 잡고, 플레이어가 있는 각도를 계산함)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // 3. 투사체 생성 (위치: firePoint, 회전: 계산된 각도)
        Instantiate(projectilePrefab, firePoint.position, rotation);
    }
}