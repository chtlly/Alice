using UnityEngine;

public class GenericNPC : MonoBehaviour
{
    // Dialogue Content
    [TextArea(3, 5)]
    public string[] dialogueLines;

    // NPC 데이터
    [Header("NPC Data")]
    public Sprite npcPortrait; // 이 NPC의 일러스트 스프라이트

    // Dialogue System References
    private GameIntroManager dialogueManager;

    // Interaction & UI Settings
    [Header("UI 및 상호작용 설정")]
    public GameObject interactionPromptUI;
    public GameObject speechBubbleObject;

    private bool isPlayerNearby = false;

    void Start()
    {
        // Manager 찾기 (최신 방식)
        dialogueManager = FindAnyObjectByType<GameIntroManager>();

        // UI 초기화
        if (interactionPromptUI != null) interactionPromptUI.SetActive(false);
        if (speechBubbleObject != null) speechBubbleObject.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Space))
        {
            if (dialogueManager.isStoryActive)
            {
                // 대화 중일 경우: Manager에게 다음 줄로 넘기라고 지시
                dialogueManager.NextLine();
            }
            else
            {
                // 대화 중이 아닐 경우: 대화가 아직 끝나지 않았을 때만 시작
                if (dialogueLines.Length > 0)
                {
                    StartDialogueSequence();
                }
            }
        }
    }

    // 대화 시작 로직 (데이터 전달 및 제어권 인계)
    void StartDialogueSequence()
    {
        if (dialogueManager == null || dialogueLines.Length == 0) return;

        // 1. Manager에게 데이터와 제어권 전달
        if (dialogueManager.portraitDisplayImage != null && npcPortrait != null)
        {
            dialogueManager.portraitDisplayImage.sprite = npcPortrait;
        }

        if (speechBubbleObject != null)
        {
            speechBubbleObject.SetActive(true);
            dialogueManager.activeSpeechBubble = speechBubbleObject;
        }

        // 2. 대화 사이클 시작: 모든 라인을 Manager에게 넘깁니다.
        dialogueManager.ShowDialogue(dialogueLines);

        // 대화 시작과 동시에 안내 UI 숨김
        if (interactionPromptUI != null) interactionPromptUI.SetActive(false);
    }

    // 플레이어 접근 감지 (OnTriggerEnter/Exit)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (interactionPromptUI != null) interactionPromptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (interactionPromptUI != null) interactionPromptUI.SetActive(false);

            // 플레이어가 멀어지면 대화가 끝나지 않았더라도 말풍선을 끕니다.
            if (speechBubbleObject != null) speechBubbleObject.SetActive(false);
        }
    }
}