using UnityEngine;

public class PlayerAttackBase : MonoBehaviour
{
    protected float damage;

    public virtual void Setup(float dmg)
    {
        this.damage = dmg;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Bossactive boss = other.GetComponent<Bossactive>();

        if (boss != null)
        {
            // 데미지 주기
            boss.TakeDamage(damage);

            // 자식들에게 "나 맞췄어!"라고 알림 (쿨타임 리셋 등을 위해)
            OnHitBoss(boss);

            // 할 일 다 했으니 삭제
            Destroy(gameObject);
        }
    }

    // 자식들이 덮어쓸 수 있는 함수
    protected virtual void OnHitBoss(Bossactive boss)
    {
    }
}