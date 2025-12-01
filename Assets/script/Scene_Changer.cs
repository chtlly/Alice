using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Changer : MonoBehaviour
{
    // 이동할 씬의 이름을 유니티 인스펙터 창에서 설정할 수 있음

    public string sceneName;
    private bool PlayerInRange = false;
    private GameObject player;

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
        if (sceneName =="shop")
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