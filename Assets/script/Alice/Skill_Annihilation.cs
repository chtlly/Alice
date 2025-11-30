using UnityEngine;

public class Skill_Annihilation : PlayerSkillBase
{
    [Header("섬멸 스킬 설정")]
    public float projectileSpeed = 15.0f;
    public float sizeMultiplier = 3.0f;
    public float maxRange = 20.0f;

    public float baseDamage = 300f;
    public float atkMultiplier = 1.1f;

    protected override void ActivateSkill()
    {
        Debug.Log("[Skill Q] 섬멸 발동! (관통 ON)");

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 dir = (mousePos - player.transform.position).normalized;

        if (skillPrefab != null)
        {
            // 1. 프리팹 생성
            GameObject obj = Instantiate(skillPrefab, player.transform.position, Quaternion.identity);

            // 2. 크기 키우기
            obj.transform.localScale *= sizeMultiplier;

            // [핵심 수정] PlayerAttack 컴포넌트 즉시 제거 (Start 실행될 틈도 안 줌)
            // DestroyImmediate를 쓰면 그 즉시 메모리에서 날려버립니다.
            PlayerAttack basicAtk = obj.GetComponent<PlayerAttack>();
            if (basicAtk != null)
            {
                DestroyImmediate(basicAtk);
            }

            // 3. 투사체 스크립트 추가
            PlayerSkillProjectile projectile = obj.AddComponent<PlayerSkillProjectile>();

            // 4. 데미지 계산 및 세팅
            float finalDamage = baseDamage + (player.Attack * atkMultiplier);

            // 관통(true) 전달 -> 이제 방해꾼(PlayerAttack)이 없으니 확실히 관통됨
            projectile.SetupProjectile(dir, projectileSpeed, finalDamage, maxRange, true);
        }
    }
}