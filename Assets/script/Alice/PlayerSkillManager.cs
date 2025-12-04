using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    [Header("Skills")]
    public PlayerSkillBase skill1;
    public PlayerSkillBase skill2;
    public PlayerSkillBase skill3;
    public PlayerSkillBase skill4;

    void Update()
    {
        if (TalkManager.instance != null && TalkManager.isTalking) return;
        if (Playeractive.instance != null && Playeractive.instance.CurrentHp <= 0) return;

        if (Input.GetKeyDown(KeyCode.Alpha1) && skill1 != null) skill1.TryUseSkill();
        if (Input.GetKeyDown(KeyCode.Alpha2) && skill2 != null) skill2.TryUseSkill();
        if (Input.GetKeyDown(KeyCode.Alpha3) && skill3 != null) skill3.TryUseSkill();
        if (Input.GetKeyDown(KeyCode.Alpha4) && skill4 != null) skill4.TryUseSkill();
    }

    public void ResetAllSkills()
    {
        if (skill1 != null) skill1.ResetCooldown();

        if (skill2 != null && skill2 is Skill_DimensionJump jumpSkill)
        {
            jumpSkill.AddStack(1);
        }
        else if (skill2 != null)
        {
            skill2.ResetCooldown();
        }

        if (skill3 != null) skill3.ResetCooldown();
    }
}