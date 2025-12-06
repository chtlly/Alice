using UnityEngine;

public class SlimeController : MonoBehaviour
{
    public float moveSpeed = 3f;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private Vector2 movement;

    // [!][추가] 스프라이트 렌더러 컴포넌트
    private SpriteRenderer spriteRenderer;

    // 게임 시작될 때 한 번만 호출되는 함수
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // [!][추가] SpriteRenderer 컴포넌트 가져오기
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

    // 매 프레임마다 호출되는 함수입니다.
    void Update()
    {
        if (playerTransform != null)
        {
            Vector3 direction = playerTransform.position - transform.position;
            direction.Normalize();
            movement = direction;

            // [!][추가] 이동 방향에 따라 스프라이트 반전
            FlipSprite(movement.x);
        }
    }

    // 고정된 시간 간격으로 물리 계산과 함께 호출되는 함수입니다.
    void FixedUpdate()
    {
        if (playerTransform != null)
        {
            moveCharacter(movement);
        }
    }

    // movement 벡터 방향으로 캐릭터를 움직이는 함수입니다.
    void moveCharacter(Vector2 direction)
    {
        rb.linearVelocity = direction * moveSpeed;
    }

    //  이동 방향에 따라 스프라이트를 좌우 반전시키는 함수
    void FlipSprite(float directionX)
    {
        // SpriteRenderer가 있는지 확인 (에러 방지)
        if (spriteRenderer == null) return;

        // 오른쪽으로 이동 중이거나 (directionX > 0),
        // 왼쪽으로 이동 중이지만 현재 오른쪽을 보고 있다면 (flipX가 false인데 directionX < 0)
        // -> 이 경우 flipX를 true로 만들어 왼쪽을 보게 함
        if (directionX > 0 && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true; // 왼쪽을 보게 반전
        }
        // 왼쪽으로 이동 중이거나 (directionX < 0),
        // 오른쪽으로 이동 중이지만 현재 왼쪽을 보고 있다면 (flipX가 true인데 directionX > 0)
        // -> 이 경우 flipX를 false로 만들어 오른쪽을 보게 함
        else if (directionX < 0 && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false; // 오른쪽을 보게 (원래 방향)
        }
        // Note: movement.x가 0 (정지)일 때는 현재 방향을 유지합니다.
    }
}