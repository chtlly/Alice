using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    [Header("스킬 슬롯 연결 (자식 오브젝트 드래그)")]
    public PlayerSkillBase skillQ; // Q 스킬 (섬멸)
    public PlayerSkillBase skillW; // W 스킬 (차원 도약)
    public PlayerSkillBase skillE; // E 스킬 (발도)
    public PlayerSkillBase skillR; // R 스킬 (리미트 브레이크)

    void Update()
    {
        // 대화 중이거나 죽었으면 스킬 사용 금지
        if (TalkManager.instance != null && TalkManager.isTalking) return;
        if (Playeractive.instance != null && Playeractive.instance.CurrentHp <= 0) return;

        // --- 키 입력 감지 ---

        // Q 키 -> 섬멸
        if (Input.GetKeyDown(KeyCode.Q) && skillQ != null)
        {
            skillQ.TryUseSkill();
        }

        // W 키 -> 차원 도약
        if (Input.GetKeyDown(KeyCode.W) && skillW != null)
        {
            skillW.TryUseSkill();
        }

        // E 키 -> 발도
        if (Input.GetKeyDown(KeyCode.E) && skillE != null)
        {
            skillE.TryUseSkill();
        }

        // R 키 -> 리미트 브레이크
        if (Input.GetKeyDown(KeyCode.R) && skillR != null)
        {
            skillR.TryUseSkill();
        }
    }
}