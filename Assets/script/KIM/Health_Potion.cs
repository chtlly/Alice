using UnityEngine;

public class Health_Potion : Item
{
    private void Awake()
    {
        itemType = ItemType.HP;
        itemName = "HP 포션";
    }
    public override bool UseEffect(Playeractive player)
    {
        if (player.CurrentHp >= player.MaxHp)
        {
            Debug.Log("체력이 이미 가득 차 있습니다!");
            return false;
        }

        player.Heal(); // 수치 변경 부탁
        return true;
    }
}
