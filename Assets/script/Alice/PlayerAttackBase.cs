using UnityEngine;

public class PlayerAttackBase : MonoBehaviour
{
    protected float damage;

    // [추가] 안전장치: 세팅이 끝나기 전엔 충돌 무시
    protected bool isSetup = false;

    // 관통 여부 스위치 (기본값: true = 맞으면 사라짐)
    public bool destroyOnHit = true;

    public virtual void Setup(float dmg)
    {
        this.damage = dmg;
        this.isSetup = true; // [핵심] 이제부터 충돌 판정 시작!
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // 1. 아직 세팅 안 됐으면 무시 (관통 설정 전에 삭제되는 것 방지)
        if (isSetup == false) return;

        // 2. 보스 확인
        Bossactive boss = other.GetComponent<Bossactive>();

        if (boss != null)
        {
            boss.TakeDamage(damage);
            OnHitBoss(boss);

            // 3. 스위치 확인: 켜져 있을 때만 삭제 (꺼져있으면 관통)
            if (destroyOnHit)
            {
                Destroy(gameObject);
            }
        }
    }

    protected virtual void OnHitBoss(Bossactive boss)
    {
    }
}