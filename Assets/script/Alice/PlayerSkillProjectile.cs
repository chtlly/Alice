using UnityEngine;

// PlayerAttackBase를 상속받음 -> 충돌 판정은 자동으로 됨!
public class PlayerSkillProjectile : PlayerAttackBase
{
    private Vector2 moveDir;
    private float moveSpeed;

    // 부모의 Setup에 더해서 방향과 속도도 받음
    public void SetupProjectile(Vector2 dir, float speed, float dmg)
    {
        // 1. 데미지는 부모한테 맡김
        base.Setup(dmg);

        // 2. 이동 설정
        moveDir = dir;
        moveSpeed = speed;

        // 3. 회전 (오른쪽 기준)
        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // 4. 안전장치: 3초 뒤 자동 삭제
        Destroy(gameObject, 3.0f);
    }

    void FixedUpdate()
    {
        // 앞으로 이동
        transform.Translate(Vector3.right * moveSpeed * Time.fixedDeltaTime);
    }
}