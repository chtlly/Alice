using UnityEngine;

public class QuickSlotController : MonoBehaviour
{
    public static QuickSlotController instance;
    public QuickSlot[] slots; // 슬롯 3개 연결됨 (0, 1, 2)
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5)) UseSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha6)) UseSlot(1);
    }
    void UseSlot(int index)
    {
        if (index < slots.Length)
        {
            slots[index].UseItem();
        }
    }

    // 아이템 타입에 따라 정해진 자리에 넣기
    public bool AddItem(Item item)
    {
        int targetIndex = -1;

        // 아이템 타입에 따라 목표 슬롯 결정
        switch (item.itemType)
        {
            case ItemType.HP:
                targetIndex = 0; // 5번 키 자리
                break;
            case ItemType.EXP:
                targetIndex = 1; // 6번 키 자리
                break;
        }

        // 목표 슬롯이 존재하면 아이템 넣기 시도
        if (targetIndex != -1 && targetIndex < slots.Length)
        {
            // 슬롯 내부에서 빈 곳이면 넣고, 같은 거면 개수 늘려줌
            return slots[targetIndex].AddItem(item);
        }

        Debug.Log("알 수 없는 아이템 타입이거나 슬롯이 없습니다.");
        return false;
    }

}