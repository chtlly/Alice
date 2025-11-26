using UnityEngine;
using UnityEngine.UI; // 이미지 제어용
using TMPro; // 텍스트 제어용

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("만들어둔 UI를 여기에 드래그하세요")]
    // 변수 이름을 유니티 오브젝트 이름과 똑같이 바꿨습니다.
    public Image Hp_Bar;
    public TextMeshProUGUI HPText;

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

    // 이 함수 이름은 그대로라 앨리스 코드 수정 안 해도 됩니다.
    public void UpdateHP(float currentHp, float maxHp)
    {
        // 1. Hp_Bar 채우기
        if (Hp_Bar != null)
        {
            Hp_Bar.fillAmount = currentHp / maxHp;
        }

        // 2. HPText 글자 바꾸기
        if (HPText != null)
        {
            HPText.text = $"{currentHp:F0} / {maxHp:F0}";
        }
    }
}