using UnityEngine;

public class PlayerSkillProjectile : PlayerAttackBase
{
    private Vector2 moveDir;
    private float moveSpeed;

    public void SetupProjectile(Vector2 dir, float speed, float dmg, float maxDistance, bool isPenetrate = false)
    {
        this.destroyOnHit = !isPenetrate;
        base.Setup(dmg);

        moveDir = dir;
        moveSpeed = speed;

        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        float calculatedLifeTime = maxDistance / speed;
        Destroy(gameObject, calculatedLifeTime);
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.fixedDeltaTime);
    }
}