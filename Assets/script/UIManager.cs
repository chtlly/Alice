using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("HP UI 연결")]
    public Image Hp_Bar;
    public TextMeshProUGUI HPText;

    [Header("Mana UI 연결")]
    public Image Mana_Bar;
    public TextMeshProUGUI ManaText;

    [Header("Exp UI 연결")]
    public Image Exp_Bar;
    public TextMeshProUGUI ExpText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateHP(float currentHp, float maxHp)
    {
        if (Hp_Bar != null) Hp_Bar.fillAmount = currentHp / maxHp;
        if (HPText != null) HPText.text = $"{currentHp:F0} / {maxHp:F0}";
    }

    public void UpdateMana(float currentMana, float maxMana)
    {
        if (Mana_Bar != null) Mana_Bar.fillAmount = currentMana / maxMana;
        if (ManaText != null) ManaText.text = $"{currentMana:F0} / {maxMana:F0}";
    }

    // [수정됨] 레벨(int level)을 인자로 추가로 받음
    public void UpdateExp(float currentExp, float maxExp, int level)
    {
        if (Exp_Bar != null)
        {
            Exp_Bar.fillAmount = currentExp / maxExp;
        }

        if (ExpText != null)
        {
            float percent = (currentExp / maxExp) * 100f;
            // 출력 형식: Lv.1 (50.00%)
            ExpText.text = $"Lv.{level} ({percent:F2}%)";
        }
    }
}