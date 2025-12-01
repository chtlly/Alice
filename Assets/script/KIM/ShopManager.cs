using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI NPCText;

    public GameObject slotPrefab;
    public Transform contentArea; // Grid Layout Group이 있는 부모 객체

    // 판매할 아이템들의 프리팹 리스트
    public List<Item> shopItems;
    void Start()
    {
        GenerateShop();
        UpdateMoneyUI();
        WelcomeMessage(); // 인사말 출력
    }
    void UpdateMoneyUI()
    {
        if (Playeractive.instance != null)
        {
            goldText.text = Playeractive.instance.gold.ToString() + "G";
        }
    }
    
    void WelcomeMessage()
    {
        ShowMessage("어서와");
    }

    public void ShowMessage(string message)
    {
        NPCText.text = message;
    }
    
    void GenerateShop()
    {
        foreach (Item item in shopItems)
        {
            GameObject newSlot = Instantiate(slotPrefab, contentArea);
            Shop slotScript = newSlot.GetComponent<Shop>();
            slotScript.Setup(item, this);
        }
    }


    public void BuyItem(Item item)
    {
        if (Playeractive.instance != null)
        {
            bool ok = Playeractive.instance.BuyItem(item);
            if (ok)
            {
                UpdateMoneyUI();
                if (string.IsNullOrEmpty(item.purchaseText) == false) // 아이템마다 대사 쓸 때 사용
                {
                    ShowMessage(item.purchaseText);
                }
                else
                {
                    ShowMessage("고마워");
                }
            }
            else
            {
                if (Playeractive.instance.gold < item.price)
                {
                    ShowMessage("돈이 부족해");
                }
                else
                {
                    ShowMessage("더이상 담을 수 없어");
                }
            }
        }
        else
        {
            return;
        }
    }
}