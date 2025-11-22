using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Bossactive : MonoBehaviour
{
    GameObject player;
    public SpriteRenderer bossrenderer;
    public Animator bossanimator;
    public float speed; //추적 속도
    public float tracestart; //추적 시작 거리
    public float traceend; //추적 중단 거리

    //보스의 스탯
    public float MaxHp;
    public float CurrentHp;
    public float ATK;
    public float BasicATK; //버프 없을 때 공격력
    public float MAXATK; //버프 받았을 때 공격력
    public bool ATKBuff; //버프 활성화 여부
    public float coolATK; //버프 쿨타임 기본 10.0f

    //보스의 상태
    public bool IsIdle = true; //아무것도 안하는 상태
    public bool IsTracing = false; //추격 중
    public bool AttackStart = false; //공격 시작 신호
    public bool IsAttacking = false; //공격 중이므로 개입하지 말라는 신호

    SkillHelper skillhelper; //스킬 쿨타임 스크립트
    public GameObject Basic_Effect; //기본 공격 이펙트
    public GameObject Break_Effect; //브레이크 어스 이펙트
    public GameObject Destiny_Effect; //운명 이펙트
    public GameObject Blood_Circle; //블러드 파운틴 이펙트

    //기본 공격
    public void Basic_ATK()
    {
        Debug.Log("기본 공격 중");
        GameObject be = Instantiate(Basic_Effect);
        //보스의 방향에 따라 생성
        if (this.bossrenderer.flipX == true)
        {
            be.transform.position = new Vector3(this.transform.position.x - 1, this.transform.position.y, 0);
        }
        else if (this.bossrenderer.flipX == false)
        {
            be.transform.position = new Vector3(this.transform.position.x + 1, this.transform.position.y, 0);
        }

    }

    //운명
    public void Destiny()
    {
        Debug.Log("운명 생성 중");
        GameObject d = Instantiate(Destiny_Effect);
        d.transform.position = this.transform.position;
    }

    //브레이크 어스
    public void Break_Earth()
    {
        Debug.Log("브레이크 어스 생성 중");
        GameObject b = Instantiate(Break_Effect);
        b.transform.position = this.transform.position;
    }

    //블러드 파운틴
    public void Blood_Fountain()
    {
        Debug.Log("블러드 파운틴 생성 중");
        GameObject c1 = Instantiate(Blood_Circle);
        c1.transform.position = this.transform.position;
    }

    //스킬 종료
    public void BossSkillend()
    {
        Debug.Log("스킬 끝");
        IsAttacking = false;
        IsIdle = true;
        this.bossanimator.SetTrigger("BossIdleTrig");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.MaxHp = 100.0f;
        this.CurrentHp = MaxHp;
        this.BasicATK = 100.0f;
        this.MAXATK = 150.0f;
        this.ATK = BasicATK;
        ATKBuff = false;
        coolATK = 0.0f;

        this.player = GameObject.Find("Player");
        this.skillhelper = GameObject.Find("SkillHelper").GetComponent<SkillHelper>();
        bossrenderer = GetComponent<SpriteRenderer>();
        bossanimator = GetComponent<Animator>();

        IsIdle = true;
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

        //공격력 버프 여부에 따라 공격력 부여
        if (ATKBuff == true)
        {
            this.ATK = MAXATK;
        }
        else
        {
            this.ATK = BasicATK;
        }

        // 아무것도 하지 않으면(공격이 끝나면) 추격 판단 모드 돌입
        if (IsIdle)
        {
            if (d >= tracestart)
            {
                Debug.Log("추격 시작");
                IsIdle = false;
                IsTracing = true;
                this.bossanimator.SetTrigger("BossWalkTrig");
                this.transform.Translate(tracedir * speed * Time.deltaTime);
            }
            else
            {
                IsIdle = false;
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
            skillhelper.BossSkillSelect();
        }
    }
}
