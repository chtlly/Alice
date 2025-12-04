using UnityEngine;
using System.Collections;

public class Skill_Passive_Wish : PlayerSkillBase
{
    [Header("패시브 설정")]
    public float triggerHpPercent = 0.5f;
    public float duration = 3.0f;

    [Header("회복 설정")]
    public float baseHeal = 10.0f;
    public float atkMultiplier = 0.6f;

    [Header("버프 설정 (공속 증가)")]
    public float atkSpeedBuffMultiplier = 1.5f;

    protected override void Update()
    {
        base.Update();

        if (player == null) return;

        bool isLowHp = (player.CurrentHp / player.MaxHp) < triggerHpPercent;

        if (currentCooldown <= 0 && isLowHp)
        {
            ActivateSkill();
        }
    }

    protected override void ActivateSkill()
    {
        currentCooldown = cooldownTime;

        if (skillPrefab != null)
        {
            GameObject effect = Instantiate(skillPrefab, player.transform.position, Quaternion.identity, player.transform);
            Destroy(effect, duration);
        }

        StartCoroutine(BuffAndHealRoutine());
    }

    IEnumerator BuffAndHealRoutine()
    {
        float originalSpeed = player.AttackSpeed;
        player.AttackSpeed *= atkSpeedBuffMultiplier;

        float totalHealAmount = baseHeal + (player.Attack * atkMultiplier);
        float healPerSecond = totalHealAmount / duration;
        float timer = 0.0f;

        while (timer < duration)
        {
            float tickHeal = healPerSecond * Time.deltaTime;
            player.Heal(tickHeal);

            timer += Time.deltaTime;
            yield return null;
        }

        player.AttackSpeed = originalSpeed;
    }
}