using UnityEngine;

public class GenericNPC : MonoBehaviour
{
    // 대화 내용 설정
    [TextArea(3, 5)]
    public string[] dialogueLines;

    // NPC 일러스트
    [Header("NPC Data")]
    public Sprite npcPortrait;

    // 대화 매니저 참조
    private GameIntroManager dialogueManager;

    // UI 설정
    [Header("UI 및 상호작용 설정")]
    public GameObject interactionPromptUI; // "F키를 누르세요" 같은 안내창
    public GameObject speechBubbleObject;  // 말풍선

    private bool isPlayerNearby = false;

    // 플레이어 제어를 위해 저장해둘 변수
    private Playeractive cachedPlayer;
    private bool isInteracting = false;

    void Start()
    {
        // 매니저 찾기 (최신 유니티 버전 호환)
        dialogueManager = FindFirstObjectByType<GameIntroManager>();
        // 만약 빨간줄 뜨면 FindAnyObjectByType<GameIntroManager>(); 로 변경

        if (interactionPromptUI != null) interactionPromptUI.SetActive(false);
        if (speechBubbleObject != null) speechBubbleObject.SetActive(false);
    }

    void Update()
    {
        // 대화 종료 감지: 내가 대화를 걸었는데(isInteracting), 매니저가 대화 끝났다고(isStoryActive == false) 하면
        if (isInteracting && dialogueManager != null && !dialogueManager.isStoryActive)
        {
            EndDialogueSequence();
        }

        // 대화 시작 입력
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Space))
        {
            if (dialogueManager.isStoryActive)
            {
                dialogueManager.NextLine(); // 다음 대사로 넘기기
            }
            else
            {
                if (dialogueLines.Length > 0)
                {
                    StartDialogueSequence(); // 대화 시작
                }
            }
        }
    }

    void StartDialogueSequence()
    {
        if (dialogueManager == null || dialogueLines.Length == 0) return;

        // [핵심] 플레이어 얼리기 & 숨기기
        if (cachedPlayer != null)
        {
            cachedPlayer.SetDialogueState(true);
        }
        isInteracting = true;

        // 매니저에게 이미지랑 대사 전달
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

    void EndDialogueSequence()
    {
        isInteracting = false;

        // [핵심] 플레이어 다시 움직이게 & 보이게 하기
        if (cachedPlayer != null)
        {
            cachedPlayer.SetDialogueState(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
            // 들어온 플레이어 스크립트 저장
            cachedPlayer = collision.GetComponent<Playeractive>();

            if (interactionPromptUI != null) interactionPromptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
            cachedPlayer = null; // 나가면 참조 해제

            if (interactionPromptUI != null) interactionPromptUI.SetActive(false);
            if (speechBubbleObject != null) speechBubbleObject.SetActive(false);
        }
    }
}