using UnityEngine;

public class Skill_DimensionJump : PlayerSkillBase
{
    [Header("차원 도약 설정")]
    public float dashDistance = 5.0f;

    [Header("스택 시스템")]
    public int maxStacks = 2;
    public int currentStacks = 2;
    public float rechargeTime = 9.0f;
    private float rechargeTimer = 0.0f;

    protected override void Start()
    {
        base.Start();
        currentStacks = maxStacks;
    }

    protected override void Update()
    {
        if (currentStacks < maxStacks)
        {
            rechargeTimer += Time.deltaTime;
            if (rechargeTimer >= rechargeTime)
            {
                currentStacks++;
                rechargeTimer = 0.0f;
                Debug.Log($"[Skill 2] 스택 충전! {currentStacks}/{maxStacks}");
            }
        }
    }

    // 부모의 TryUseSkill을 무시하고 내 방식(스택)으로 씀
    public override void TryUseSkill()
    {
        if (currentStacks <= 0)
        {
            Debug.Log($"[Skill 2] 스택 부족... ({rechargeTimer:F1}초 남음)");
            return;
        }

        if (player != null && player.UseMana(manaCost))
        {
            ActivateSkill();
        }
    }

    protected override void ActivateSkill()
    {
        Debug.Log("[Skill 2] 차원 도약!");
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
}