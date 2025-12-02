using UnityEngine;

public class furAttack : MonoBehaviour
{
    public float speed = 5f;   // 투사체 속도
    public int damage = 1;     // 투사체 데미지
    public float lifeTime = 3f; // 3초 후 자동 삭제 (무한히 날아가는 것 방지)

    void Start()
    {
        // 생성된 후 일정 시간이 지나면 스스로 사라짐
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // 오른쪽(Red Axis) 방향으로 계속 날아감
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}