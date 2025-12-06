using UnityEngine;

public class SlimeController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float stopDistance = 1.0f;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private Vector2 movement;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("플레이어 오브젝트를 찾을 수 없습니다! 'Player' 태그가 설정되었는지 확인하세요.");
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // 플레이어와 슬라임 사이의 거리 계산
            float distance = Vector2.Distance(transform.position, playerTransform.position);

            // [!][수정] 거리가 stopDistance보다 멀 때만 이동 계산
            if (distance > stopDistance)
            {
                Vector3 direction = playerTransform.position - transform.position;
                direction.Normalize();
                movement = direction;

                // 이동 중일 때만 방향 전환
                FlipSprite(movement.x);
            }
            else
            {
                // [!][추가] 거리가 가까우면 멈춤 (플레이어와 겹치지 않게 함)
                movement = Vector2.zero;
            }
        }
    }

    void FixedUpdate()
    {
        if (playerTransform != null)
        {
            moveCharacter(movement);
        }
    }

    void moveCharacter(Vector2 direction)
    {
        // 방향이 0이면(멈춤 상태면) 속도도 0이 됨
        rb.linearVelocity = direction * moveSpeed;
    }

    void FlipSprite(float directionX)
    {
        if (spriteRenderer == null) return;

        // 오른쪽으로 이동 중(>0)인데, 현재 반전이 안 되어 있다면(!flipX = 왼쪽 보는 중)
        // -> 반전시켜서 오른쪽을 보게 함 (True)
        if (directionX > 0 && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
        }
        // 왼쪽으로 이동 중(<0)인데, 현재 반전이 되어 있다면(flipX = 오른쪽 보는 중)
        // -> 반전을 꺼서 원래대로 왼쪽을 보게 함 (False)
        else if (directionX < 0 && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
        }
    }
}