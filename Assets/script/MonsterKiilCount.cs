using UnityEngine;

public class MonsterKillCount : MonoBehaviour
{
 
    public static MonsterKillCount instance;

    [Header("게임 현황")]
    public int currentKillCount = 0; // 현재 잡은 몬스터 수

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
    }

    // 몬스터가 죽을 때 호출할 함수
    public void AddKillCount()
    {
        currentKillCount++;
        Debug.Log($"몬스터 처치! 현재 킬 수: {currentKillCount}");
    }
}