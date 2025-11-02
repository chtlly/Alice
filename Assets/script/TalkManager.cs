using UnityEngine;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour
{
    public static TalkManager instance;

    public GameObject talkPanel;
    public Text talkText;

    public static bool isTalking; // 대화 중인지 확인하는 
    public Image npcPortrait; // 이미지를 담을 변수

    private string[] talkData; // 여러 대사를 담을 배열
    private int talkIndex; // 현재 대화 순서

    

    private void Awake()
    {


        // 인스턴스가 없으면 자신을 할당
        if (instance == null)
        {
            instance = this;

            //DontDestroyOnLoad(gameObject);
        }
      

    }

    void Start()
    {
        talkPanel.SetActive(false);
        npcPortrait.gameObject.SetActive(false);
    }

    // 새로운 함수: 대화 시작
    public void StartTalk(string[] talks, Sprite npcSprite)
    {
        talkData = talks; // 전달받은 대사를 배열에 저장
        talkIndex = 0; // 대화 순서를 0으로 초기화
        isTalking = true; // 대화 시작

        talkPanel.SetActive(true); // 대화창 활성화
        talkText.text = talkData[talkIndex]; // 첫 번째 대사 출력

        // NPC 일러스트가 있으면 활성화하고 이미지를 할당
        npcPortrait.gameObject.SetActive(true);
        npcPortrait.sprite = npcSprite;
        npcPortrait.color = Color.white;
        

    }

    // 새로운 함수: 다음 대화로 넘기기
    public void NextTalk()
    {
        talkIndex++; // 대화 순서를 1 증가

        // 모든 대사가 끝났는지 확인
        if (talkIndex >= talkData.Length)
        {
            isTalking = false; // 대화 끝
            talkPanel.SetActive(false); // 끝났으면 대화창 비활성화

            if (npcPortrait != null)
            {
                npcPortrait.gameObject.SetActive(false);
            }
        }
        else
        {
            talkText.text = talkData[talkIndex]; // 아니면 다음 대사 출력
        }

        if (talkIndex >= talkData.Length)
        {
            talkPanel.SetActive(false);
        }

    }
}