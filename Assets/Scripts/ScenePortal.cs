using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour
{
    private bool PlayerInRange = false;
    [Header("다음 씬 설정")]
    // ⭐ 이동할 씬의 빌드 인덱스 번호를 Inspector에 입력하세요. ⭐
    //public int nextSceneIndex;
    public string sceneName;
    [Header("트리거 설정")]
    // 플레이어 오브젝트에 "Player" 태그가 붙어있어야 합니다.
    public string playerTag = "Player";

    private GameObject player;

    // 플레이어가 포탈 영역에 진입했을 때 호출됩니다.
    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. 진입한 오브젝트의 태그가 플레이어인지 확인
        if (other.CompareTag(playerTag))
        {
            Debug.Log($"플레이어가 포탈에 진입했습니다. 씬 인덱스 {nextSceneIndex}로 이동합니다.");

            // 2. 인덱스 번호를 사용하여 씬 로드
            LoadNextSceneByIndex();
        }
    }
    
    // 씬을 로드하는 전용 함수
    private void LoadNextSceneByIndex()
    {
        // 빌드 설정에 해당 인덱스의 씬이 등록되어 있는지 확인하는 간단한 검사
        if (nextSceneIndex >= 0 && nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // ⭐ 인덱스 번호로 씬을 로드합니다. ⭐
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogError($"씬 로드 실패: 인덱스 {nextSceneIndex}는 빌드 설정에 등록되지 않았거나 유효하지 않습니다.");
        }
    }
    */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = true; // 범위 안에 들어옴
            player = other.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = false;
            player = null;
        }
    }
    private void ChangeScene()
    {
        if (sceneName == "Shop")
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            player.GetComponent<Playeractive>().enabled = false;
        }
        else
        {
            // 맵 이동한거 확인하기 위해 디버그
            Debug.Log("플레이어가 이동했습니다.");
            SceneManager.LoadScene(sceneName);
        }
    }
    private void Update()
    {
        if (PlayerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            ChangeScene();
        }
    }
}