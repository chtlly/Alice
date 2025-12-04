using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float lifeTime = 0.5f;

    void Start()
    {
        Invoke("DestroyEffect", lifeTime);
    }

    void DestroyEffect()
    {
        if (Playeractive.instance != null)
        {
            Playeractive.instance.ResetAttackCooldown();
        }
        Destroy(gameObject);
    }
}
