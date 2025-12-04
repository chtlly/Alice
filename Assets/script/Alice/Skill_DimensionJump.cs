using UnityEngine;

public class Skill_DimensionJump : PlayerSkillBase
{
    [Header("차원 도약 설정")]
    public float dashDistance = 5.0f;

    [Header("스택(충전) 시스템")]
    public int maxStacks = 2;
    public int currentStacks = 2;
    public float rechargeTime = 9.0f;

    private float rechargeTimer = 0.0f;

    private float internalCooldown = 0.5f;
    private float internalTimer = 0.0f;

    protected override void Start()
    {
        base.Start();
        currentStacks = maxStacks;
    }

    protected override void Update()
    {
        if (internalTimer > 0) internalTimer -= Time.deltaTime;

        if (currentStacks < maxStacks)
        {
            rechargeTimer += Time.deltaTime;

            if (rechargeTimer >= rechargeTime)
            {
                currentStacks++;
                rechargeTimer = 0.0f;
            }
        }
    }

    public override void TryUseSkill()
    {
        if (internalTimer > 0) return;

        if (currentStacks <= 0)
        {
            return;
        }

        if (player != null && player.UseMana(manaCost))
        {
            ActivateSkill();
        }
    }

    protected override void ActivateSkill()
    {
        internalTimer = internalCooldown;
        currentStacks--;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 dir = (mousePos - player.transform.position).normalized;

        if (skillPrefab != null)
        {
            GameObject effect = Instantiate(skillPrefab, player.transform.position, Quaternion.identity);
            Destroy(effect, 0.2f);
        }

        Vector3 targetPos = player.transform.position + (Vector3)(dir * dashDistance);
        player.transform.position = targetPos;
        player.LookAt(mousePos);
    }

    public void AddStack(int amount)
    {
        currentStacks += amount;
        if (currentStacks > maxStacks) currentStacks = maxStacks;
    }

    public override float GetCurrentCooldown()
    {
        if (currentStacks >= maxStacks) return 0;
        return 1.0f - (rechargeTimer / rechargeTime);
    }

    public override int GetStackCount()
    {
        return currentStacks;
    }
}