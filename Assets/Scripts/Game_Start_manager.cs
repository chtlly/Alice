using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class Game_Start_manager : MonoBehaviour
{
    // ⭐ 씬 이동 시 다음 씬에서 스토리를 자동 재생해야 하는지 알려주는 플래그 ⭐
    public static bool shouldAutoPlayStory = false;

    // 씬 내 싱글톤 패턴
    public static Game_Start_manager Instance;

    void Awake()
    {
        Instance = this;
    }

    [Header("Scene & Game Flow Settings")]
    [Tooltip("이동할 게임 씬의 이름을 Build Settings에 있는 그대로 입력하세요.")]
    public string gameSceneName = "GameScene";

    [Header("UI Panels")]
    [Tooltip("메인 화면(타이틀 씬)에서만 연결하세요. (선택 사항)")]
    public GameObject titlePanel;
    [Tooltip("스토리/대화창 패널. 필수 연결입니다.")]
    public GameObject storyPanel;

    [Header("Story Settings")]
    [Tooltip("스토리 텍스트 출력을 위해 필수 연결입니다.")]
    public TextMeshProUGUI storyText;
    public float typingSpeed = 0.05f;

    [TextArea(3, 5)]
    public string[] storyLines;

    private int currentLineIndex = 0;
    private bool isStoryActive = false;
    private bool isTyping = false;

    // ⭐ 씬 로드 시점의 초기화 및 자동 재생 판별 ⭐
    void Start()
    {
        // 1. 새 씬에 도착했고, 직전에 '게임 시작' 버튼을 눌렀다면
        if (shouldAutoPlayStory)
        {
            OnClickShowStory(autoStart: true);
            // 플래그를 바로 초기화하여 이후 씬 로드 시에도 계속 재생되는 것을 방지
            shouldAutoPlayStory = false;
        }
        else
        {
            // 타이틀 씬이거나, 일반적인 씬 로드 시의 초기화
            // 스토리 패널은 기본적으로 비활성화 상태여야 함 (수동 또는 버튼 클릭으로 활성화)
            if (storyPanel != null)
            {
                storyPanel.SetActive(false);
            }
            // titlePanel이 있다면 활성화 상태로 유지
            if (titlePanel != null)
            {
                titlePanel.SetActive(true);
            }
        }
    }

    // '스토리 보기' 버튼 또는 Start()에서 호출되는 함수 (스토리 시작 로직)
    public void OnClickShowStory(bool autoStart = false)
    {
        Debug.Log(autoStart ? "새 씬 로드 후 스토리 자동 시작" : "스토리 버튼 클릭됨!");

        if (storyPanel == null || storyText == null)
        {
            Debug.LogError("핵심 UI 컴포넌트(storyPanel 또는 storyText)가 연결되지 않았습니다! 스토리를 출력할 수 없습니다.");
            return;
        }

        // titlePanel이 연결되어 있을 때만 비활성화 (타이틀 씬에서 타이틀 숨기기)
        if (titlePanel != null)
        {
            titlePanel.SetActive(false);
        }

        // 스토리 패널 활성화
        storyPanel.SetActive(true);

        isStoryActive = true;
        currentLineIndex = 0;

        StartCoroutine(TypeStoryLine());
    }

    // ⭐ '게임 시작' 버튼 전용 함수: 씬 이동 전 플래그를 설정합니다. ⭐
    public void OnClickStartGame()
    {
        Debug.Log("게임 시작 버튼 클릭! 다음 씬을 로드합니다.");
        shouldAutoPlayStory = true; // 다음 씬에서 스토리를 자동 재생하도록 플래그 설정
        LoadGameScene();
    }

    void Update()
    {
        if (!isStoryActive) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // 타이핑 스킵
                StopAllCoroutines();
                if (storyText != null)
                {
                    storyText.text = storyLines[currentLineIndex];
                }
                isTyping = false;
            }
            else
            {
                // 다음 라인 진행
                NextLine();
            }
        }
    }

    void NextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < storyLines.Length)
        {
            StartCoroutine(TypeStoryLine());
        }
        else
        {
            // 대사가 끝났다면 -> 스토리 종료 및 UI 복귀
            EndStory();
        }
    }

    IEnumerator TypeStoryLine()
    {
        isTyping = true;
        if (storyText == null)
        {
            isTyping = false;
            yield break;
        }

        storyText.text = "";
        string currentLine = storyLines[currentLineIndex];

        for (int i = 0; i < currentLine.Length; i++)
        {
            if (!isTyping) break;

            storyText.text += currentLine[i];
            yield return new WaitForSeconds(typingSpeed);
        }

        if (storyText.text != currentLine)
        {
            storyText.text = currentLine;
        }

        isTyping = false;
    }

    // 스토리 종료 로직 (씬 이동 없음, 타이틀 패널은 연결되어 있을 때만 복귀)
    void EndStory()
    {
        isStoryActive = false;

        // 스토리 창 닫기
        if (storyPanel != null) storyPanel.SetActive(false);

        // titlePanel이 연결되어 있을 때만 활성화 (타이틀 씬 복귀)
        if (titlePanel != null)
        {
            titlePanel.SetActive(true);
        }

        Debug.Log("스토리 재생이 종료되었습니다. UI를 초기 상태로 되돌립니다.");
    }

    // 씬 로드 전용 함수
    private void LoadGameScene()
    {
        if (string.IsNullOrEmpty(gameSceneName))
        {
            Debug.LogError("게임 씬 이름이 설정되지 않았습니다! 인스펙터의 'Game Scene Name'을 확인해주세요.");
            return;
        }

        SceneManager.LoadScene(gameSceneName);
    }
}