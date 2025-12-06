using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopExit : MonoBehaviour
{
    // 현재 상점 씬 자신의 이름을 적어주세요 (Inspector에서 설정)
    public string currentSceneName;

    public void CloseShop()
    {
        // [핵심] 현재 열려있는 이 상점 씬만 닫습니다 (Main 씬은 뒤에 그대로 있음)
        SceneManager.UnloadSceneAsync(currentSceneName);

        // 아까 멈췄던 플레이어를 다시 움직이게 하려면 여기서 코드를 추가해야 합니다.
        GameObject.FindWithTag("Player").GetComponent<Playeractive>().enabled = true;
    }
}
