using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour
{
    [Header("다음 씬 설정")]
    // ⭐ 이동할 씬의 빌드 인덱스 번호를 Inspector에 입력하세요. ⭐
    public int nextSceneIndex;

    [Header("클리어 조건")]
    public int targetKillCount = 50; // 목표 처치 수

    [Header("트리거 설정")]
    // 플레이어 오브젝트에 "Player" 태그가 붙어있어야 합니다.
    public string playerTag = "Player";

    // 플레이어가 포탈 영역에 진입했을 때 호출됩니다.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            // 1. 게임 매니저가 있는지 확인
            if (MonsterKillCount.instance == null)
            {
                Debug.LogError("MonsterKillCount가 씬에 없습니다!");
                return;
            }

            // 2. 킬 카운트가 목표치에 도달했는지 확인
            if (MonsterKillCount.instance.currentKillCount >= targetKillCount)
            {
                Debug.Log($"목표 달성! ({MonsterKillCount.instance.currentKillCount}/{targetKillCount}) 다음 스테이지로 이동합니다.");
                LoadNextSceneByIndex();
            }
            else
            {
                // 아직 부족할 때
                int left = targetKillCount - MonsterKillCount.instance.currentKillCount;
                Debug.Log($"아직 포탈을 탈 수 없습니다. {left}마리 더 잡으세요!");
            }
        }
    }


    // 씬을 로드하는 전용 함수
    private void LoadNextSceneByIndex()
    {
        // 빌드 설정에 해당 인덱스의 씬이 등록되어 있는지 확인하는 간단한 검사
        if (nextSceneIndex >= 0 && nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // 다음 스테이지로 넘어가면 몬스터 킬 카운트 초기화
            if (MonsterKillCount.instance != null)
            {
                MonsterKillCount.instance.currentKillCount = 0;
            }
            // ⭐ 인덱스 번호로 씬을 로드합니다. ⭐
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogError($"씬 로드 실패: 인덱스 {nextSceneIndex}는 빌드 설정에 등록되지 않았거나 유효하지 않습니다.");
        }
    }
}