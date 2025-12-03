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

        // 1. 보스 때리기
        Bossactive boss = other.GetComponent<Bossactive>();
        if (boss != null)
        {
            boss.TakeDamage(damage);
            OnHitBoss(boss);
            if (destroyOnHit) Destroy(gameObject);
            return; // 보스 맞췄으면 끝
        }

        // 2. [추가] 몬스터 때리기
        MonsterStats monster = other.GetComponent<MonsterStats>();
        if (monster != null)
        {
            // 몬스터는 int형 체력을 쓰므로 형변환
            monster.TakeDamage((int)damage);

            // 몬스터도 맞으면 투사체가 사라지게 할지 결정
            if (destroyOnHit) Destroy(gameObject);
        }
    }

    protected virtual void OnHitBoss(Bossactive boss)
    {
    }
}