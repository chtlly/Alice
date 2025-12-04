using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillHUD : MonoBehaviour
{
    [Header("스킬 매니저 연결")]
    public PlayerSkillManager skillManager;

    [Header("쿨타임 오버레이 (검은색 막)")]
    public Image cooldownImage1;
    public Image cooldownImage2;
    public Image cooldownImage3;
    public Image cooldownImage4;

    [Header("스택 텍스트 (2번 스킬용)")]
    public TextMeshProUGUI skill2StackText;

    void Start()
    {
        // 자동으로 플레이어 찾기
        if (skillManager == null)
        {
            GameObject player = GameObject.Find("Player");
            if (player != null) skillManager = player.GetComponent<PlayerSkillManager>();
        }
    }

    void Update()
    {
        if (skillManager == null) return;

        // 1. 쿨타임 오버레이 갱신
        if (skillManager.skill1 != null && cooldownImage1 != null)
        {
            cooldownImage1.fillAmount = skillManager.skill1.GetCurrentCooldown();
        }

        if (skillManager.skill2 != null && cooldownImage2 != null)
        {
            cooldownImage2.fillAmount = skillManager.skill2.GetCurrentCooldown();
        }

        if (skillManager.skill3 != null && cooldownImage3 != null)
        {
            cooldownImage3.fillAmount = skillManager.skill3.GetCurrentCooldown();
        }

        if (skillManager.skill4 != null && cooldownImage4 != null)
        {
            cooldownImage4.fillAmount = skillManager.skill4.GetCurrentCooldown();
        }

        // 2. 스택 텍스트 갱신 (2번 스킬)
        if (skillManager.skill2 != null && skill2StackText != null)
        {
            int stacks = skillManager.skill2.GetStackCount();

            if (stacks >= 0) // 스택이 있는 스킬이면 표시
            {
                if (!skill2StackText.gameObject.activeSelf)
                    skill2StackText.gameObject.SetActive(true);

                skill2StackText.text = stacks.ToString();
            }
            else
            {
                if (skill2StackText.gameObject.activeSelf)
                    skill2StackText.gameObject.SetActive(false);
            }
        }
    }
}