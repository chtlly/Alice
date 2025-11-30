using UnityEngine;
using UnityEngine.UI; // 이미지 제어용
using TMPro; // 텍스트 제어용

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("HP UI 연결")]
    public Image Hp_Bar;
    public TextMeshProUGUI HPText;

    [Header("Mana UI 연결")] // [추가됨]
    public Image Mana_Bar;
    public TextMeshProUGUI ManaText;

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
        if (Hp_Bar != null)
        {
            Hp_Bar.fillAmount = currentHp / maxHp;
        }

        if (HPText != null)
        {
            HPText.text = $"{currentHp:F0} / {maxHp:F0}";
        }
    }

    // [추가됨] 마나 업데이트 함수
    public void UpdateMana(float currentMana, float maxMana)
    {
        if (Mana_Bar != null)
        {
            Mana_Bar.fillAmount = currentMana / maxMana;
        }

        if (ManaText != null)
        {
            ManaText.text = $"{currentMana:F0} / {maxMana:F0}";
        }
    }
}