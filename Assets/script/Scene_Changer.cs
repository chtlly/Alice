using UnityEngine;
using UnityEngine.SceneManagement; // 씬을 이동하기 위해서 필요한 네임스페이스

public class Scene_Changer : MonoBehaviour
{
    // 이동할 씬의 이름을 유니티 인스펙터 창에서 설정할 수 있음
    public string sceneName; // 이동할 씬의 이름 받기

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 만약 들어온 오브젝트의 태그가 Player면
        if (other.CompareTag("Player")) {
            // 맵 이동한거 확인하기 위해 디버그
            Debug.Log("플레이어가 이동했습니다.");

            // 지정된 씬으로 이동하기
            SceneManager.LoadScene(sceneName);

        }
    }
}
