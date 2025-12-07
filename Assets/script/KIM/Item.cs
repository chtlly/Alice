using UnityEngine;
using System.Collections.Generic;

public enum ItemType
{
    HP,
    EXP
}

public abstract class Item : MonoBehaviour
{
    public int itemNumber;
    public string itemName;
    public Sprite itemImage;

    public int count;

    public int price;
    public int maxCount;
    public ItemType itemType;

    // 혹시나 아이템마다 대사 등록할려면
    [TextArea]
    public string purchaseText;
    public abstract bool UseEffect(Playeractive player);
}
