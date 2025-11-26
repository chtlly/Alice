using UnityEngine;

public class Blood_Blossom : BossAttackBase
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
            // 1. 데미지 계산 (최대 체력 15%)
            float rawDamage = player.MaxHp * 0.15f;

            // 2. 데미지 적용 후 실질적인 피해량 리턴받음
            float dealtDamage = player.TakeDamage(rawDamage);

            Debug.Log($"핏빛 매화 적중! (흡혈량: {dealtDamage})");

            // 3. 보스 회복 (입힌 데미지 100%)
            if (bossactive != null)
            {
                bossactive.Heal(dealtDamage);
            }
        }
    }
}