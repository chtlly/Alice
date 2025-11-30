using UnityEngine;

public class PlayerSkillProjectile : PlayerAttackBase
{
    private Vector2 moveDir;
    private float moveSpeed;

    public void SetupProjectile(Vector2 dir, float speed, float dmg, float maxDistance, bool isPenetrate = false)
    {
        // 1. 관통 설정 먼저 함
        this.destroyOnHit = !isPenetrate;

        // 2. 부모 Setup 호출 (데미지 설정 + 안전장치 해제!)
        // 이 시점에는 이미 destroyOnHit가 false로 세팅되어 있어서 안전함
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
        // Unity 2023 이상은 linearVelocity, 구버전은 velocity
        // GetComponent<Rigidbody2D>().velocity = moveDir * moveSpeed; 
        transform.Translate(Vector3.right * moveSpeed * Time.fixedDeltaTime);
    }
}