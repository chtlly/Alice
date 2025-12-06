using UnityEngine;

public class furAttack : MonoBehaviour
{
    [Header("투사체 설정")]
    public float speed = 5f;       // 날아가는 속도
    public int damage = 10;        // 데미지
    public float maxRange = 10f;   // [추가] 사거리 (이 거리만큼 날아가면 사라짐)

    void Start()
    {
        // [핵심] 사거리 제한 구현
        // (거리 / 속도 = 도달하는 데 걸리는 시간)
        // 예: 10m / 5속도 = 2초 뒤 삭제
        float lifeTime = maxRange / speed;

        // 계산된 시간 뒤에 자동으로 사라짐
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // 오른쪽 방향으로 계속 이동
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어에게 닿았을 때만 반응
        if (other.CompareTag("Player"))
        {
            Playeractive player = other.GetComponent<Playeractive>();
            if (player != null)
            {
                player.TakeDamage(damage); // 데미지 줌
            }
            Destroy(gameObject); // 플레이어 맞췄으면 사라짐
        }

    }
}