using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    [Header("스킬 슬롯 연결")]
    public PlayerSkillBase skill1; // 섬멸
    public PlayerSkillBase skill2; // 차원 도약 (예외 처리 대상)
    public PlayerSkillBase skill3; // 발도
    public PlayerSkillBase skill4; // 리미트 브레이크

    void Update()
    {
        if (TalkManager.instance != null && TalkManager.isTalking) return;
        if (Playeractive.instance != null && Playeractive.instance.CurrentHp <= 0) return;

        if (Input.GetKeyDown(KeyCode.Alpha1) && skill1 != null) skill1.TryUseSkill();
        if (Input.GetKeyDown(KeyCode.Alpha2) && skill2 != null) skill2.TryUseSkill();
        if (Input.GetKeyDown(KeyCode.Alpha3) && skill3 != null) skill3.TryUseSkill();
        if (Input.GetKeyDown(KeyCode.Alpha4) && skill4 != null) skill4.TryUseSkill();
    }

    // [추가됨] 리미트 브레이크가 호출할 함수
    public void ResetAllSkills()
    {
        Debug.Log(">> [리미트 브레이크 효과] 스킬 초기화 시작");

        // 1번(섬멸): 쿨타임 초기화
        if (skill1 != null) skill1.ResetCooldown();

        // 2번(차원 도약): 쿨타임 초기화가 아니라 '스택 +1'
        // skill2가 Skill_DimensionJump 인지 확인하고 맞으면 변환해서 기능 사용
        if (skill2 != null && skill2 is Skill_DimensionJump jumpSkill)
        {
            jumpSkill.AddStack(1); // 스택 1개 충전
        }
        else if (skill2 != null)
        {
            skill2.ResetCooldown();     
        }

        if (skill3 != null) skill3.ResetCooldown();

    }
}