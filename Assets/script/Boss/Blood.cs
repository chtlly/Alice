using UnityEngine;

public class Blood : BossAttackBase
{
    void Update()
    {
        if (bossactive != null && bossactive.IsAttacking == false)
        {
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        Playeractive player = other.GetComponent<Playeractive>();
        if (player != null)
        {
            // 1. 데미지 (최대 체력 15%)
            float damageAmount = player.MaxHp * 0.15f;
            Debug.Log($"블러드 파운틴 적중! (데미지: {damageAmount})");
            player.TakeDamage(damageAmount);

            // 2. 보스 버프 (공격력 50% 증가, 10초)
            if (bossactive != null)
            {
                bossactive.ApplyAtkBuff(1.5f, 10.0f);
            }
        }
    }
}