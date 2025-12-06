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
            float damageAmount = player.MaxHp * 0.15f;
            player.TakeDamage(damageAmount);

            if (bossactive != null)
            {
                bossactive.ApplyAtkBuff(1.5f, 10.0f);
            }
        }
    }
}