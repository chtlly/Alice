using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
//using static Player_Status;
public class Playeractive : CharacterBase
{
    [Header("Gold")]
    public int gold = 1000; // 테스트

    public Vector2 inputVec;
    public float speed;
    public static Playeractive instance; // 싱글톤 인스턴스

    Rigidbody2D rigid; //물리 연산을 담당
    SpriteRenderer spriter; // 스프라이트 이미지 렌더링
    Animator anim; // 애니메이션
    
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        //변수 초기화
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        
        // 인스턴스가 없으면 자신을 할당하고, 파괴되지 않게 설정
        instance = this;
        DontDestroyOnLoad(gameObject);

        // 씬이 로드될 때마다 실행될 함수 연결
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    void Start()
    {
        InitializeStats(100, 10); // 나중에 값 변경
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 새로운 씬에서 "StartPoint" 오브젝트 찾기
        GameObject startPoint = GameObject.Find("StartPoint");

        // startPoint가 존재하면 캐릭터의 위치를 그곳으로 이동
        if (startPoint != null)
        {
            transform.position = startPoint.transform.position;
        }
    }

    public bool BuyItem(Item item)
    {
        if (gold < item.price)
        {
            Debug.Log("돈이 부족합니다");
            return false;
        }
        else {
            if (QuickSlotController.instance.AddItem(item))
            {
                gold -= item.price;
                Debug.Log($"구매 {item.itemName}. 남은 돈: {gold}");
                return true;
            }
            else
            {
                Debug.Log("슬롯을 확인해 주세요");
                return false;
            }
        }
    }
    void Update()
    {
        // 매 프레임마다 호출되는 함수 
        if (TalkManager.instance != null && TalkManager.isTalking)
        {
            inputVec = Vector2.zero; // 입력 벡터를 0으로 초기화
            return; // 함수종료
        }

        inputVec.x = Input.GetAxisRaw("Horizontal"); // 좌우 받기
        inputVec.y = Input.GetAxisRaw("Vertical"); // 상하 받기

    }

    void FixedUpdate()
    {// 물리 연산하기 위해 일정 시간마다 사용하는 함수

        if (TalkManager.instance != null && TalkManager.isTalking)
            return;

       Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; 
       rigid.MovePosition(rigid.position + nextVec); //실시간 위치 업데이트
    }

    private void LateUpdate()
    {// 카메라나 애니메이션처럼 다른 오브젝트의 행동에 영향을 받을 경우에 사용

        if (TalkManager.instance != null && TalkManager.isTalking)
        {
            anim.SetFloat("Speed", 0);
            return;
        }

        anim.SetFloat("Speed", inputVec.magnitude); //Speed 상태일 때 바꿔줘라


        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }
}


