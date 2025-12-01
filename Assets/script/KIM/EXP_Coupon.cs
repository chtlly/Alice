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
        return true;
    }
    /*
    public override bool UseEffect(Playeractive player)
    {
        if (player.CurrentExp >= player.MaxExp)
        {
            Debug.Log("체력이 이미 가득 차 있습니다!");
            return false;
        }
        player.Exp_up(30);
    }
    */
}
