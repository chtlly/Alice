using UnityEngine;

public class Destiny : BossAttackBase
{
    public float time = 2.0f;

    protected override void Start()
    {
        base.Start();

        // 생성 즉시 버프 적용 (공격력 50% 증가, 10초)
        if (bossactive != null)
        {
            Debug.Log("운명 발동! (공격력 50% 증가)");
            bossactive.ApplyAtkBuff(1.5f, 10.0f);
        }
    }

    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            Destroy(gameObject);
            if (bossactive != null) bossactive.BossSkillend();
        }
    }
}