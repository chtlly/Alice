using UnityEngine;

public class BossAttackBase : MonoBehaviour
{
    protected Bossactive bossactive;

    protected virtual void Start()
    {
        // 보스 연결
        GameObject bossObj = GameObject.Find("Rabbit_Ssi");
        if (bossObj != null)
        {
            bossactive = bossObj.GetComponent<Bossactive>();
        }
    }

    // [중요] 자식들이 덮어쓸 수 있도록 virtual로 선언
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Playeractive player = other.GetComponent<Playeractive>();

        if (player != null)
        {
            // 기본 동작: 보스의 현재 공격력(ATK)으로 데미지를 줌
            // 스킬마다 데미지 방식이 다르므로(최대 체력 비례 등), 자식에서 override해서 쓸 예정
            if (bossactive != null)
            {
                player.TakeDamage(bossactive.ATK);
            }

            // 추가 효과 발동
            OnHitPlayerEffect(player);
        }
    }

    protected virtual void OnHitPlayerEffect(Playeractive player)
    {
        // 기본은 빈칸 (자식에서 구현)
    }
}