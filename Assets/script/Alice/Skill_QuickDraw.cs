using UnityEngine;
using System.Collections;

public class Skill_QuickDraw : PlayerSkillBase
{
    [Header("발도 설정")]
    public float damageDuration = 0.3f;
    public float spawnDistance = 1.5f;

    public float baseDamage = 350f;
    public float atkMultiplier = 0.8f;

    public float buffDuration = 3.0f;
    public float atkBuffMultiplier = 1.5f;

    protected override void ActivateSkill()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 dir = (mousePos - player.transform.position).normalized;

        if (player != null)
        {
            player.LookAt(mousePos);
            Animator playerAnim = player.GetComponent<Animator>();
            if (playerAnim != null) playerAnim.SetTrigger("Attack");

            player.SetGlobalDelay(damageDuration);
        }

        Vector3 spawnPos = player.transform.position + (Vector3)(dir * spawnDistance);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle - 90);

        if (skillPrefab != null)
        {
            GameObject obj = Instantiate(skillPrefab, spawnPos, rotation);

            PlayerAttack basicAtk = obj.GetComponent<PlayerAttack>();
            if (basicAtk != null) DestroyImmediate(basicAtk);

            PlayerAttackBase attack = obj.AddComponent<PlayerAttackBase>();
            float finalDamage = baseDamage + (player.Attack * atkMultiplier);
            attack.destroyOnHit = false;
            attack.Setup(finalDamage, damageDuration);
        }

        StartCoroutine(BuffRoutine());
    }

    IEnumerator BuffRoutine()
    {
        float originalAttack = player.Attack;
        player.Attack *= atkBuffMultiplier;
        yield return new WaitForSeconds(buffDuration);
        player.Attack = originalAttack;
    }
}