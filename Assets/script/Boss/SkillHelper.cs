using UnityEngine;

public class SkillHelper : MonoBehaviour
{
    Bossactive bossactive;
    GameObject player;

    // 쿨타임 원본(max)과 현재 쿨타임
    public float cool1m, cool2m, cool3m, cool4m;
    public float cool1, cool2, cool3, cool4;

    void Start()
    {
        // [HEAD 방식 채택] 안전하게 보스와 플레이어 찾기
        GameObject bossObj = GameObject.Find("Rabbit_Ssi");
        if (bossObj != null) bossactive = bossObj.GetComponent<Bossactive>();
        player = GameObject.Find("Player");

        // [공통] 초기 쿨타임 설정
        cool1m = 15.0f; // 운명
        cool2m = 12.0f; // 브레이크 어스
        cool3m = 10.0f; // 블러드 파운틴
        cool4m = 15.0f; // 핏빛 매화

        // 게임 시작 시 바로 스킬을 쓸 수 있게 초기 쿨타임은 짧게 설정
        cool1 = 5.0f; cool2 = 5.0f; cool3 = 5.0f; cool4 = 5.0f;
    }

    void Update()
    {
        // [HEAD 방식 채택] 보스나 플레이어가 없으면 로직 중단 (에러 방지)
        if (bossactive == null || player == null) return;

        // [신규 로직] 실시간 체력 비율 계산
        float hpPercent = bossactive.CurrentHp / bossactive.MaxHp * 100f;

        // 보스 체력에 따른 쿨타임 감소 로직
        if (hpPercent <= 30f)
        {
            // 체력 30% 이하: 운명(10초), 브레이크어스(9초)로 감소
            cool1m = 10.0f;
            cool2m = 9.0f;
        }
        else
        {
            cool1m = 15.0f;
            cool2m = 12.0f;
        }

        // 쿨타임 감소 진행
        cool1 -= Time.deltaTime; cool2 -= Time.deltaTime; cool3 -= Time.deltaTime; cool4 -= Time.deltaTime;
    }

    public void BossSkillSelect()
    {
        if (bossactive == null || player == null) return;

        // 플레이어와의 거리 (5m 이내 확인)
        float distance = Vector2.Distance(bossactive.transform.position, player.transform.position);
        bool isNear = distance <= 5.0f;
        float hpPercent = bossactive.CurrentHp / bossactive.MaxHp * 100f;

        int skillnumber = Random.Range(1, 5); // 1~4 랜덤 선택

        // 1. 운명 (조건: 쿨타임 완료 + 거리 5m 이내)
        if (skillnumber == 1 && cool1 <= 0 && isNear)
        {
            cool1 = cool1m;
            Debug.Log("운명 발동");
            bossactive.bossanimator.SetTrigger("BossDesTrig");
        }
        // 2. 브레이크 어스 (조건: 쿨타임 완료 + 거리 5m 이내)
        else if (skillnumber == 2 && cool2 <= 0 && isNear)
        {
            cool2 = cool2m;
            Debug.Log("브레이크 어스 발동");
            bossactive.bossanimator.SetTrigger("BossBreakTrig");
        }
        // 3. 블러드 파운틴 (조건: 체력 30% 이하 + 거리 5m 이내)
        else if (skillnumber == 3 && cool3 <= 0 && isNear && hpPercent <= 30f)
        {
            cool3 = cool3m;
            Debug.Log("블러드 파운틴 발동");
            bossactive.bossanimator.SetTrigger("BossBloodTrig");
        }
        // 4. 핏빛 매화 (조건: 체력 10% 이하 + 거리 5m 이내)
        else if (skillnumber == 4 && cool4 <= 0 && isNear && hpPercent <= 10f)
        {
            cool4 = cool4m;
            Debug.Log("핏빛 매화 발동");
            bossactive.blood_cool = 0.5f;
            bossactive.blood_level = 0;
            bossactive.IsBlood = true;
        }
        else
        {
            // 조건이 안 맞거나 쿨타임이면 기본 공격
            Debug.Log("기본 공격");
            bossactive.bossanimator.SetTrigger("BossAtkTrig");
        }
    }
}