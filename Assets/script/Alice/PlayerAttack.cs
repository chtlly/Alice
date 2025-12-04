using UnityEngine;

public class PlayerAttack : PlayerAttackBase
{
    public float lifeTime = 0.5f;

    void Start()
    {
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