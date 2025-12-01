using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class InteractionTrigger : MonoBehaviour
{
    [Header("대화 내용 (여러 줄)")]
    public string[] dialogueText; // 문자열 배열로 변경

    public GameObject talkPromptUI;
    private SpriteRenderer npcRanderer;

    // NPC 일러스트 담기
    public Sprite npcPortrait;

    private bool isPlayerInRange = false;

    private void Awake()
    {
        npcRanderer = GetComponent<SpriteRenderer>();
    }


    private void Start()
    {
        if (talkPromptUI == null)
        {
            // 주의: Hierarchy에 있는 실제 말풍선 오브젝트의 이름과 똑같아야 합니다.
            GameObject foundObj = GameObject.Find("TalkPromptUI");

            if (foundObj != null)
            {
                talkPromptUI = foundObj;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (talkPromptUI != null)
            {
                talkPromptUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (talkPromptUI != null && talkPromptUI.activeInHierarchy)
            {
                talkPromptUI.SetActive(false);
            }

            if (TalkManager.instance != null)
            {
                TalkManager.instance.talkPanel.SetActive(false);
            }

        }
    }

    private void Update()
    {
        // 플레이어가 범위 안에 있고(isPlayerInRange) 스페이스 바를 누르면
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            // 대화창이 이미 활성화되어 있으면 다음 대화로 넘기기
            if (TalkManager.instance.talkPanel.activeSelf)
            {
                TalkManager.instance.NextTalk();
            }
            else // 대화창이 비활성화 상태라면 (처음 대화 시작) & npc Sprite 넘겨주기
            {
                TalkManager.instance.StartTalk(dialogueText, npcPortrait);
                if (talkPromptUI != null)
                {
                    talkPromptUI.SetActive(false);
                }
            }

        }
    }
}