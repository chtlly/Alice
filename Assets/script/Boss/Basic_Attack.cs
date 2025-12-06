using UnityEngine;

public class Basic_Attack : BossAttackBase
{
    SpriteRenderer direction;

    protected override void Start()
    {
        base.Start();

        this.direction = GetComponent<SpriteRenderer>();
        if (bossactive != null)
        {
            if (bossactive.bossrenderer.flipX == false)
            {
                this.direction.flipX = true;
            }
            else
            {
                this.direction.flipX = false;
            }
        }
    }

    void Update()
    {
        if (bossactive != null && bossactive.IsAttacking == false)
        {
            Destroy(gameObject);
        }
    }
}