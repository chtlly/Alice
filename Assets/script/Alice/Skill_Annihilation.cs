using UnityEngine;

public class Skill_Annihilation : PlayerSkillBase
{
    [Header("¼¶¸ê ½ºÅ³ ¼³Á¤")]
    public float projectileSpeed = 15.0f;
    public float sizeMultiplier = 3.0f;
    public float maxRange = 20.0f;

    public float baseDamage = 300f;
    public float atkMultiplier = 1.1f;

    protected override void ActivateSkill()
    {
        Debug.Log("[Skill 1] ¼¶¸ê ¹ßµ¿!");

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 dir = (mousePos - player.transform.position).normalized;

        if (player != null)
        {
            player.LookAt(mousePos);
            Animator playerAnim = player.GetComponent<Animator>();
            if (playerAnim != null) playerAnim.SetTrigger("Attack");

            // [Ãß°¡] 0.5ÃÊ µ¿¾È ÆòÅ¸ ±ÝÁö
            player.SetGlobalDelay(0.5f);
        }

        if (skillPrefab != null)
        {
            GameObject obj = Instantiate(skillPrefab, player.transform.position, Quaternion.identity);
            obj.transform.localScale *= sizeMultiplier;

            PlayerAttack basicAtk = obj.GetComponent<PlayerAttack>();
            if (basicAtk != null) DestroyImmediate(basicAtk);

            PlayerSkillProjectile projectile = obj.AddComponent<PlayerSkillProjectile>();
            float finalDamage = baseDamage + (player.Attack * atkMultiplier);
            projectile.SetupProjectile(dir, projectileSpeed, finalDamage, maxRange, true);
        }
    }
}