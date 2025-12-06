using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Image itemIcon;
    public Button purchase;
    public Text itemName;
    public Text itemPrice;


    private Item itemData;
    private ShopManager shopManager;

    // 상점 매니저가 이 함수를 호출해서 정보를 세팅
    public void Setup(Item item, ShopManager manager)
    {
        itemData = item;
        shopManager = manager;

        if (item.itemImage != null)
        {
            itemIcon.sprite = item.itemImage;
        }
        itemName.text = item.itemName;
        itemPrice.text = item.price.ToString() + " Gold";

        
        purchase.onClick.RemoveAllListeners();
        // 버튼 클릭 시 구매 시도
        purchase.onClick.AddListener(() => {
            shopManager.BuyItem(itemData);
        
        });
        
    }
    /*
    void OnBuyClick()
    {
        shopManager.BuyItem(itemData);
    }
    */
}
