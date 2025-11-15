using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Bossactive : MonoBehaviour
{
    GameObject player;
    SpriteRenderer bossrenderer;
    Animator bossanimator;
    public float speed; //속도
    public float tracestart; //추적 시작 거리
    public float traceend; //추적 중단 거리
    public int skillnumber; // 어떤 스킬을 쓸 지(테스트를 위해 외부에서도 지정 가능)

    //보스의 상태
    public bool IsTracing = false; //추격 중
    public bool AttackStart = false; //공격 시작 신호
    public bool IsAttacking = false; //공격 중이므로 개입하지 말라는 신호

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.player = GameObject.Find("Player");
        bossrenderer = GetComponent<SpriteRenderer>();
        bossanimator = GetComponent<Animator>();
    }

    //어떤 스킬을 쓸 지 선택하는 함수
    public void BossSkillSelect()
    {
        skillnumber = Random.Range(0, 5);
        if (skillnumber == 0)
        {
            Debug.Log("기본 공격");
            this.bossanimator.SetTrigger("BossAtkTrig");
        }
        else if (skillnumber == 1)
        {
            Debug.Log("브레이크 어스");
            this.bossanimator.SetTrigger("BossBreakTrig");
        }
        else if (skillnumber == 2)
        {
            Debug.Log("운명");
            BossSkillend();
        }
        else if (skillnumber == 3)
        {
            Debug.Log("블러드 파운틴");
            this.bossanimator.SetTrigger("BossBloodTrig");
        }
        else if (skillnumber == 4)
        {
            Debug.Log("핏빛 매화");
            BossSkillend();
        }
    }

    public void BossSkillend()
    {
        Debug.Log("스킬 끝");
        IsAttacking = false;
        this.bossanimator.SetTrigger("BossIdleTrig");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pb = this.transform.position; //보스의 위치
        Vector2 pp = this.player.transform.position; //플레이어의 위치
        Vector2 tracedir = pp - pb; //보스에서 플레이어를 향하는 벡터
        float d = tracedir.magnitude; //보스에서 플레이어까지의 거리

        //벡터 방향에 따라 보스 스프라이트 뒤집기
        if (tracedir.x < 0)
        {
            bossrenderer.flipX = true;
        }
        else
        {
            bossrenderer.flipX = false;
        }

        // 아무것도 하지 않으면(공격이 끝나면) 추격 판단 모드 돌입
        if (!IsTracing && !AttackStart && !IsAttacking)
        {
            if (d >= tracestart && !IsTracing)
            {
                Debug.Log("추격 시작");
                IsTracing = true;
                this.bossanimator.SetTrigger("BossWalkTrig");
                this.transform.Translate(tracedir * speed * Time.deltaTime);
            }
            else
            {
                AttackStart = true;
            }
        }
        //특정 거리로 좁혀질 때까지 추격
        else if (IsTracing)
        {
            if (d >= traceend)
            {
                Debug.Log("추격 중");
                this.transform.Translate(tracedir * speed * Time.deltaTime);
            }
            else if (d <= traceend)
            {
                Debug.Log("추격 중단");
                IsTracing = false;
                AttackStart = true;
                this.bossanimator.SetTrigger("BossIdleTrig");
            }
        }
        //공격 시작
        else if (AttackStart)
        {
            Debug.Log("공격 시작");
            AttackStart = false; //반복되지 않게끔 문 닫기
            IsAttacking = true;
            BossSkillSelect();
        }
    }
}
