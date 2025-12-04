using UnityEngine;

public class SceneStoryTrigger : MonoBehaviour
{
    [TextArea(5, 10)]
    public string[] scenePrologueLines; // ⭐ 이 씬에서 보여줄 스토리 대사 입력 ⭐

    void Start()
    {
        // 씬이 시작되자마자 GameIntroManager를 찾습니다.
        GameIntroManager manager = FindAnyObjectByType<GameIntroManager>();

        if (manager != null && scenePrologueLines.Length > 0)
        {
            // 찾았다면, 이 씬의 스토리 대사 목록을 Manager에게 넘겨 바로 출력하도록 지시합니다.
            manager.ShowDialogue(scenePrologueLines);
        }
    }
}