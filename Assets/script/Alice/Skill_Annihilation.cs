using UnityEngine;

public class Skill_Annihilation : PlayerSkillBase
{
    [Header("섬멸 스킬 설정")]
    public float projectileSpeed = 15.0f;
    public float sizeMultiplier = 3.0f;

    public float baseDamage = 300f;
    public float atkMultiplier = 1.1f;

    protected override void ActivateSkill()
    {
        Debug.Log("[Skill Q] 섬멸 발동!");

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 dir = (mousePos - player.transform.position).normalized;

        if (skillPrefab != null)
        {
            // 1. 생성
            GameObject obj = Instantiate(skillPrefab, player.transform.position, Quaternion.identity);

            // 2. 크기 키우기
            obj.transform.localScale *= sizeMultiplier;

            // 3. 기존 평타 스크립트 끄기 (충돌 중복 방지)
            PlayerAttack basicAtk = obj.GetComponent<PlayerAttack>();
            if (basicAtk != null) basicAtk.enabled = false;

            // 4. [핵심] 투사체 스크립트 붙이기
            // 날아가는 기능이 필요하므로 PlayerSkillProjectile을 붙임
            PlayerSkillProjectile projectile = obj.AddComponent<PlayerSkillProjectile>();

            // 5. 데미지 계산
            float finalDamage = baseDamage + (player.Attack * atkMultiplier);

            // 6. 세팅 (방향, 속도, 데미지)
            projectile.SetupProjectile(dir, projectileSpeed, finalDamage);
        }
    }
}