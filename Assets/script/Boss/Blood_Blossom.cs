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
        if (player != null && bossactive.CurrentHp > 0)
        {
            float rawDamage = player.MaxHp * 0.15f;

            float dealtDamage = player.TakeDamage(rawDamage);

            if (bossactive != null)
            {
                bossactive.Heal(dealtDamage*0.10f);
            }
        }
    }
}