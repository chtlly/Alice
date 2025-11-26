using UnityEngine;
using UnityEngine.UIElements;

public class SkillHelper : MonoBehaviour
{
    //스킬에 필요한 것들을 관리하는 클래스

    Bossactive bossactive;
    public int phase; //보스의 체력에 따른 페이스
    public int skillphase; //쓸 수 있는 스킬 수 + 1
    public int skillnumber; //쓸 스킬 번호
    public float cool1m, cool2m, cool3m, cool4m; //각 스킬 쿨타임 리셋용 수치
    public float cool1, cool2, cool3, cool4; //각 스킬 쿨타임
    public float testcool; //테스트용 쿨타임

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossactive = GameObject.Find("Rabbit_Ssi").GetComponent<Bossactive>();
        phase = 1;
        skillphase = 3;
        cool1m = 15.0f; cool2m = 12.0f; cool3m = 10.0f; cool4m = 15.0f;
        //1: 15.0f->10.0f / 2: 12.0f->9.0f / 3: 10.0f; / 4: 15.0f;
        cool1 = 5.0f; cool2 = 5.0f; cool3 = 5.0f; cool4 = 5.0f; //첫 쿨타임은 게임의 재미를 위해 5초로 설정.

        //테스트용
        /*testcool = 2.0f;
        skillnumber = 1;
        cool1 = testcool;*/
    }

    // Update is called once per frame
    void Update()
    {
        //체력에 따른 페이스 변화
        /*if (bossactive.CurrentHp <= (bossactive.MaxHp / 100) * 30 && bossactive.CurrentHp > (bossactive.MaxHp / 100) * 10)
        {
            phase = 2; skillphase = 4;
            cool1m = 10.0f; cool2m = 9.0f;
            cool3 = 5.0f;
        }
        else if (bossactive.CurrentHp <= (bossactive.MaxHp / 100) * 10)
        {
            phase = 3; skillphase = 5;
            cool4 = 5.0f;
        }*/

        //스킬 쿨타임 감소
        cool1 -= Time.deltaTime; cool2 -= Time.deltaTime; cool3 -= Time.deltaTime; cool4 -= Time.deltaTime;

        //공격 버프 지속 시간
        if (bossactive.ATKBuff == true)
        {
            if (bossactive.coolATK >= 0)
            {
                bossactive.coolATK -= Time.deltaTime;
            }
            else
            {
                bossactive.ATKBuff = false;
            }
        }
    }

    public void BossSkillSelect()
    {
        //skillnumber = 4;
        skillnumber = Random.Range(1, 5); // 랜덤 테스트용
        //skillnumber = Random.Range(1, skillphase); //페이스에 따른 스킬 선택
        if (skillnumber == 1 && cool1 <= 0)
        {
            cool1 = cool1m;
            Debug.Log("운명");
            bossactive.bossanimator.SetTrigger("BossDesTrig");
        }
        else if (skillnumber == 2 && cool2 <= 0)
        {
            cool2 = cool2m;
            Debug.Log("브레이크 어스");
            bossactive.bossanimator.SetTrigger("BossBreakTrig");
        }
        else if (skillnumber == 3 && cool3 <= 0)
        {
            cool3 = cool3m;
            Debug.Log("블러드 파운틴");
            bossactive.bossanimator.SetTrigger("BossBloodTrig");
        }
        else if (skillnumber == 4 && cool4 <= 0)
        {
            cool4 = cool4m;
            Debug.Log("핏빛 매화");
            bossactive.blood_cool = 0.5f;
            bossactive.blood_level = 0;
            bossactive.IsBlood = true;
        }
        else
        {
            Debug.Log("기본 공격");
            bossactive.bossanimator.SetTrigger("BossAtkTrig");
        }
    }
}
