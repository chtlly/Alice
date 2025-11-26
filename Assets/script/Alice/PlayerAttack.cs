using UnityEngine;

// [중요] PlayerAttackBase를 상속받습니다.
public class PlayerAttack : PlayerAttackBase
{
    public float lifeTime = 0.5f;

    void Start()
    {
        // 기본 공격력 전달 (부모에게)
        if (Playeractive.instance != null)
        {
            base.Setup(Playeractive.instance.Attack);
        }

        // 일정 시간 후 삭제 (허공에 공격했을 때)
        Invoke("DestroyEffect", lifeTime);
    }

    // 1. 허공에 공격해서 시간이 다 되어 사라질 때
    void DestroyEffect()
    {
        if (Playeractive.instance != null)
        {
            Playeractive.instance.ResetAttackCooldown(); // 쿨타임 해제
        }
        Destroy(gameObject);
    }

    // 2. 보스를 맞춰서 사라질 때 (부모가 호출해줌)
    protected override void OnHitBoss(Bossactive boss)
    {
        // 보스를 때렸을 때도 쿨타임을 풀어줘야 다음 공격이 나감
        if (Playeractive.instance != null)
        {
            Playeractive.instance.ResetAttackCooldown();
        }
    }
}