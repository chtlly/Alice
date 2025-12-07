using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI NPCText;

    public GameObject slotPrefab;
    public Transform contentArea; // Grid Layout Group이 있는 부모 객체

    public static ShopManager Instance;

    [Header("UI 연결")]
    public GameObject confirmPanel;   // 확인 팝업창 패널
    public TextMeshProUGUI questionText; // "000을 500원에 살래?" 텍스트

    // 판매할 아이템들의 프리팹 리스트
    public List<Item> shopItems;

    private Item selectedItem;

    void Awake()
    {
        Instance = this;
    }
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
            MoneyText.text = Playeractive.instance.Money.ToString() + "G";
        }
    }
    
    void WelcomeMessage()
    {
        ShowMessage("어서와~ 앨리스.\n뭐 찾는 거 있어 ? ");
        //ShowMessage("앨리스, 지금 \n가진 돈이 부족해");
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

    // 아이템을 클릭했을 때 호출되는 함수
    public void ShowConfirmation(Item item)
    {
        selectedItem = item; // 아이템 정보 저장

        // 팝업창 텍스트 갱신
        questionText.text = $"{item.itemName}을(를)\n<color=yellow>{item.price}G</color>에 구매하시겠습니까?";

        confirmPanel.SetActive(true); // 팝업창 켜기
    }

    // 팝업창의 '구매(Yes)' 버튼에 연결할 함수
    public void OnClickBuy(Item item)
    {
        bool ok = Playeractive.instance.BuyItem(item);

        if (ok)
        {

            Debug.Log("구매 성공! 남은 돈: " + Playeractive.instance.Money);
            UpdateMoneyUI();
            // TODO: 인벤토리에 아이템 추가하는 코드 넣기

            ClosePanel();
        }
        else
        {
            Debug.Log("돈이 부족합니다!");
            UpdateMoneyUI();
            ClosePanel();
        }
    }

    // 팝업창의 '취소(No)' 버튼에 연결할 함수
    public void OnClickCancel()
    {
        ClosePanel();
    }

    void ClosePanel()
    {
        confirmPanel.SetActive(false);
        selectedItem = null;
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
                    ShowMessage("매번 사줘서\n고마워~");
                }
            }
            else
            {
                if (Playeractive.instance.Money < item.price)
                {
                    ShowMessage("앨리스, 지금 \n가진 돈이 부족해");
                }
            }
        }
        else
        {
            return;
        }
    }
    
}
/*
public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance; // 어디서든 부르기 쉽게 싱글톤 처리

    [Header("플레이어 데이터")]
    public int playerGold = 1000; // 현재 가진 돈 (테스트용 1000원)

    [Header("UI 연결")]
    public GameObject confirmPanel;   // 확인 팝업창 패널
    public TextMeshProUGUI questionText; // "000을 500원에 살래?" 텍스트

    private ShopItem selectedItem; // 지금 살려고 고민 중인 아이템 저장용

    void Awake()
    {
        Instance = this;
    }

    // 아이템을 클릭했을 때 호출되는 함수
    public void ShowConfirmation(ShopItem item)
    {
        selectedItem = item; // 아이템 정보 저장

        // 팝업창 텍스트 갱신
        questionText.text = $"{item.itemName}을(를)\n<color=yellow>{item.price}G</color>에 구매하시겠습니까?";

        confirmPanel.SetActive(true); // 팝업창 켜기
    }

    // 팝업창의 '구매(Yes)' 버튼에 연결할 함수
    public void OnClickBuy()
    {
        if (playerGold >= selectedItem.price)
        {
            playerGold -= selectedItem.price;
            Debug.Log("구매 성공! 남은 돈: " + playerGold);

            // TODO: 인벤토리에 아이템 추가하는 코드 넣기

            ClosePanel();
        }
        else
        {
            Debug.Log("돈이 부족합니다!");
            // 여기에 "돈 부족해!" 알림창을 띄워도 됨
            ClosePanel();
        }
    }

    // 팝업창의 '취소(No)' 버튼에 연결할 함수
    public void OnClickCancel()
    {
        ClosePanel();
    }

    void ClosePanel()
    {
        confirmPanel.SetActive(false);
        selectedItem = null;
    }
}
*/