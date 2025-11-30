using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    [Header("스킬 슬롯 연결")]
    public PlayerSkillBase skill1; // 1번 (섬멸)
    public PlayerSkillBase skill2; // 2번 (차원 도약)
    public PlayerSkillBase skill3; // 3번 (발도)
    public PlayerSkillBase skill4; // 4번 (리미트 브레이크)

    void Update()
    {
        if (TalkManager.instance != null && TalkManager.isTalking) return;
        if (Playeractive.instance != null && Playeractive.instance.CurrentHp <= 0) return;

        // 키 입력 1~4
        if (Input.GetKeyDown(KeyCode.Alpha1) && skill1 != null) skill1.TryUseSkill();
        if (Input.GetKeyDown(KeyCode.Alpha2) && skill2 != null) skill2.TryUseSkill();
        if (Input.GetKeyDown(KeyCode.Alpha3) && skill3 != null) skill3.TryUseSkill();
        if (Input.GetKeyDown(KeyCode.Alpha4) && skill4 != null) skill4.TryUseSkill();
    }
}