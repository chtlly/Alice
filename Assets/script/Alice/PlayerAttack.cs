using UnityEngine;

public class PlayerAttack : PlayerAttackBase
{
    public float lifeTime = 0.5f;

    void Start()
    {
        if (Playeractive.instance != null)
        {
            // 부모의 Setup을 호출해서 공격력 전달 + 안전장치 해제(isSetup = true)
            base.Setup(Playeractive.instance.Attack);
        }
        Invoke("DestroyEffect", lifeTime);
    }

    void DestroyEffect()
    {
        if (Playeractive.instance != null)
        {
            Playeractive.instance.ResetAttackCooldown();
        }
        Destroy(gameObject);
    }

    protected override void OnHitBoss(Bossactive boss)
    {
        if (Playeractive.instance != null)
        {
            Playeractive.instance.ResetAttackCooldown();
        }
    }
}