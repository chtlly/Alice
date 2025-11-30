using UnityEngine;

public class PlayerAttack : PlayerAttackBase
{
    public float lifeTime = 0.5f;

    void Start()
    {
        // Playeractive에서 InitAttack을 호출해줄 때까지 대기
    }

    public void InitAttack(float damage, float duration)
    {
        base.Setup(damage);
        Invoke("DestroyEffect", duration);
    }

    void DestroyEffect()
    {
        Destroy(gameObject);
    }
}