using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    public Image hpBarFront;

    private Transform parentTransform;
    private Vector3 originalScale;

    void Start()
    {
        // [수정] 부모가 있는지 먼저 확인
        if (transform.parent != null)
        {
            parentTransform = transform.parent;
        }
        else
        {
            // 부모가 없으면 경고를 띄우고, 에러가 안 나게 자기 자신을 넣음
            // Debug.LogWarning("BossHPBar: 부모(보스)가 없습니다! Hierarchy를 확인하세요.");
            parentTransform = transform;
        }

        originalScale = transform.localScale;
    }

    void LateUpdate()
    {
        // [수정] 부모가 없거나 사라졌으면(null) 아무것도 하지 않음 (에러 방지)
        if (parentTransform == null) return;

        // 보스가 뒤집히면 체력바도 같이 뒤집히는 걸 방지
        if (parentTransform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }
        else
        {
            transform.localScale = originalScale;
        }
    }

    public void UpdateHP(float currentHp, float maxHp)
    {
        if (hpBarFront != null)
        {
            hpBarFront.fillAmount = currentHp / maxHp;
        }
    }
}