using UnityEngine;
using System.Collections;

public class Skill_LimitBreak : PlayerSkillBase
{
    [Header("Limit Break Settings")]
    public float buffDuration = 5.0f;
    public float atkSpeedMultiplier = 1.8f;

    private PlayerSkillManager skillManager;

    protected override void Start()
    {
        base.Start();
        skillManager = GetComponentInParent<PlayerSkillManager>();
        if (skillManager == null && player != null)
        {
            skillManager = player.GetComponent<PlayerSkillManager>();
        }
    }

    protected override void ActivateSkill()
    {
        if (skillPrefab != null)
        {
            GameObject effect = Instantiate(skillPrefab, player.transform.position, Quaternion.identity, player.transform);
            effect.transform.localPosition = new Vector3(0, 4.5f, 0);
            Destroy(effect, 1.0f);
        }

        StartCoroutine(ActivationRoutine());
    }

    IEnumerator ActivationRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        if (skillManager != null)
        {
            skillManager.ResetAllSkills();
        }

        float originalAttackSpeed = player.AttackSpeed;
        player.AttackSpeed *= atkSpeedMultiplier;

        yield return new WaitForSeconds(buffDuration);

        player.AttackSpeed = originalAttackSpeed;
    }
}