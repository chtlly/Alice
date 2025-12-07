using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public string itemName = "체력 물약";
    public int price = 100;

    // 마우스로 이 오브젝트를 클릭했을 때 자동 실행
    /*
    private void OnMouseDown()
    {
        // 매니저에게 "나 이거 사고 싶어"라고 알림
        ShopManager.Instance.ShowConfirmation(this);
    }
    */
}