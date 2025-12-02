using UnityEngine;

public class GenericNPC : MonoBehaviour
{
    // Dialogue Content
    [TextArea(3, 5)]
    public string[] dialogueLines;

    // NPC 데이터
    [Header("NPC Data")]
    public Sprite npcPortrait;

    // Dialogue System References
    private GameIntroManager dialogueManager;

    // Interaction & UI Settings
    [Header("UI 및 상호작용 설정")]
    public GameObject interactionPromptUI;
    public GameObject speechBubbleObject;

    private bool isPlayerNearby = false;

    // [추가] 플레이어 스크립트 참조 저장
    private Playeractive cachedPlayer;
    // [추가] 현재 이 NPC와 대화가 진행 중인지 체크
    private bool isInteracting = false;

    void Start()
    {
        dialogueManager = FindAnyObjectByType<GameIntroManager>();

        if (interactionPromptUI != null) interactionPromptUI.SetActive(false);
        if (speechBubbleObject != null) speechBubbleObject.SetActive(false);
    }

    void Update()
    {
        // [추가] 대화 종료 감지 로직
        // 내가 대화를 시작했는데(isInteracting), Manager가 대화 끝났다고 하면(isStoryActive == false)
        if (isInteracting && dialogueManager != null && !dialogueManager.isStoryActive)
        {
            EndDialogueSequence();
        }

        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Space))
        {
            if (dialogueManager.isStoryActive)
            {
                dialogueManager.NextLine();
            }
            else
            {
                if (dialogueLines.Length > 0)
                {
                    StartDialogueSequence();
                }
            }
        }
    }

    void StartDialogueSequence()
    {
        if (dialogueManager == null || dialogueLines.Length == 0) return;

        // [추가] 대화 시작 시 플레이어 상태 변경 (투명 + 이동불가)
        if (cachedPlayer != null)
        {
            cachedPlayer.SetDialogueState(true);
        }
        isInteracting = true; // 대화 시작됨 표시

        // Manager에게 데이터 전달
        if (dialogueManager.portraitDisplayImage != null && npcPortrait != null)
        {
            dialogueManager.portraitDisplayImage.sprite = npcPortrait;
        }

        if (speechBubbleObject != null)
        {
            speechBubbleObject.SetActive(true);
            dialogueManager.activeSpeechBubble = speechBubbleObject;
        }

        dialogueManager.ShowDialogue(dialogueLines);

        if (interactionPromptUI != null) interactionPromptUI.SetActive(false);
    }

    // [추가] 대화가 끝났을 때 호출되는 함수
    void EndDialogueSequence()
    {
        isInteracting = false; // 대화 종료 표시

        // 플레이어 상태 원상복구 (보임 + 이동가능)
        if (cachedPlayer != null)
        {
            cachedPlayer.SetDialogueState(false);
        }

        // (참고: 말풍선 끄기나 UI 처리는 Manager가 하거나 OnTriggerExit에서 처리됨)
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
            // [추가] 들어온 플레이어 스크립트 저장해두기
            cachedPlayer = collision.GetComponent<Playeractive>();

            if (interactionPromptUI != null) interactionPromptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
            cachedPlayer = null; // 나갔으니 참조 해제

            if (interactionPromptUI != null) interactionPromptUI.SetActive(false);
            if (speechBubbleObject != null) speechBubbleObject.SetActive(false);
        }
    }
}