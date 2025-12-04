using UnityEngine;

public class PlayerAttackBase : MonoBehaviour
{
    protected float damage;
    public bool destroyOnHit = true;
    protected bool isSetup = false;

    public virtual void Setup(float dmg, float duration = 0f)
    {
        this.damage = dmg;
        this.isSetup = true;

        if (duration > 0)
        {
            Destroy(gameObject, duration);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (isSetup == false) return;

        Bossactive boss = other.GetComponent<Bossactive>();
        if (boss != null)
        {
            boss.TakeDamage(damage);
            OnHitBoss(boss);

            if (destroyOnHit) Destroy(gameObject, 0.1f);
            return;
        }

        MonsterStats monster = other.GetComponent<MonsterStats>();
        if (monster != null)
        {
            monster.TakeDamage((int)damage);

            if (destroyOnHit) Destroy(gameObject, 0.1f);
        }
    }

    protected virtual void OnHitBoss(Bossactive boss)
    {
    }
}