using UnityEngine;

public class EXP_Coupon :Item
{
    private void Awake()
    {
        itemNumber = 2;
        itemName = "경험치 쿠폰";
    }
    
    public override bool UseEffect(Playeractive player)
    {
        player.GainExp(10);
        return true;
    }
    
}
