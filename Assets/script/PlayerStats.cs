using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Data")]
    public int level = 1;
    public int currentExp = 0;
    public int maxExp = 100;
    public int gold = 0;

    // 몬스터가 죽을 때 호출할 함수 (보상 수령)
    public void GetReward(int exp, int goldAmount)
    {
        // 1. 경험치와 골드 추가
        currentExp += exp;
        gold += goldAmount;

        Debug.Log($" 보상 획득 [경험치 +{exp}] [골드 +{goldAmount}]");

        // 2. 레벨업 체크 (경험치가 꽉 찼는지?)
        CheckLevelUp();

        // 상태 로그 출력
        Debug.Log($"현재 상태: Lv.{level} | EXP: {currentExp}/{maxExp} | Gold: {gold}");
    }

    void CheckLevelUp()
    {
        // 현재 경험치가 최대 경험치보다 많거나 같으면 레벨업
        while (currentExp >= maxExp)
        {
            currentExp -= maxExp; // 남은 경험치는 다음 레벨로 이월
            level++;              // 레벨 증가
            maxExp += 50;         // 다음 레벨 필요 경험치 증가 

            Debug.Log($" 레벨 업! 현재 레벨: {level}");
        }
    }
}