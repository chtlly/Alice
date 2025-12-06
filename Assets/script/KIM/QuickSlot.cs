using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuickSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI count;

    private Item currentItem;
    private int currentCount;

    public Item defaultItem;

    public bool Full = false;
    public bool HasItem
    {
        get
        {
            return currentItem != null;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (defaultItem != null)
        {
            AddItem(defaultItem);
        }
        else
        {
            ClearSlot();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public bool AddItem(Item newItem)
    {
        if (currentItem == null)
        { 
            currentItem = newItem;
            currentCount = 0;
            icon.sprite = newItem.itemImage;
            icon.enabled = true;
            UpdateCountUI();
            return true;
        }
        else if (currentItem.itemName == newItem.itemName)
        {
            if (currentCount < currentItem.maxCount)
            {
                currentCount++;
                UpdateCountUI();
                return true;
            }
            else
            {
                Debug.Log("더 이상 가질 수 없습니다");
                return false;
            }
        }
        return false;
    }

    // 아이템 비우기 (사용 후 호출)
    void ClearSlot()
    {
        currentItem = null;
        currentCount = 0;
        icon.sprite = null;
        icon.enabled = false;
        if (count != null) count.text = "";
    }

    public void UseItem()
    {
        if (currentItem == null || currentCount <= 0) return;

        // 플레이어에게 효과 적용 시도
        if (currentItem.UseEffect(Playeractive.instance))
        {
            // 성공 시 개수 감소
            currentCount--;

            if (currentCount <= 0)
            {
                //ClearSlot();
                UpdateCountUI();
            }
            else
            {
                UpdateCountUI();
            }
        }
    }
    void UpdateCountUI()
    {
        if (count != null)
        {
            count.text = currentCount.ToString();
        }
    }
}
