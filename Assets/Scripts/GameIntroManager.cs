using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
// using UnityEngine.SceneManagement; // 씬 전환 로직 불필요

public class GameIntroManager : MonoBehaviour
{
    // ⭐ 1. 씬 내 싱글톤 패턴 (Static Instance) ⭐
    // 이 씬에서 NPC들이 쉽게 이 관리자를 찾을 수 있도록 합니다.
    public static GameIntroManager Instance;

    void Awake()
    {
        // 씬 로드 시마다 자기 자신을 Instance로 지정
        // (이 씬에는 GameIntroManager 오브젝트가 하나만 있어야 합니다.)
        Instance = this;
    }

    [Header("UI Panels")]
    [Tooltip("각 씬의 인스펙터에서 직접 연결해야 합니다.")]
    public GameObject titlePanel;
    [Tooltip("각 씬의 인스펙터에서 직접 연결해야 합니다.")]
    public GameObject storyPanel;

    [Header("Story Settings")]
    [Tooltip("각 씬의 인스펙터에서 직접 연결해야 합니다.")]
    public TextMeshProUGUI storyText;
    public float typingSpeed = 0.05f;

    [TextArea(3, 5)]
    public string[] storyLines;

    // NPC로부터 제어권을 넘겨받은 UI들
    [Header("Active Dialogue Control")]
    [Tooltip("각 씬의 인스펙터에서 직접 연결해야 합니다.")]
    public Image portraitDisplayImage;       // 일러스트 Image 컴포넌트 연결
    [Tooltip("각 씬의 인스펙터에서 직접 연결해야 합니다.")]
    public GameObject portraitPanelObject;    // 일러스트 패널 자체 연결
    public GameObject activeSpeechBubble;      // 현재 활성화된 NPC의 말풍선 오브젝트 (NPC가 설정)

    private int currentLineIndex = 0;
    public bool isStoryActive { get; private set; } = false;
    public bool isTyping { get; private set; } = false;

    // NPC가 새로운 대화 목록을 넘겨줄 때 호출됩니다.
    public void ShowDialogue(string[] newLines)
    {
        // 모든 UI 요소가 인스펙터에서 연결되었는지 확인합니다.
        if (storyText == null || storyPanel == null)
        {
            Debug.LogError("UI 컴포넌트 연결 실패! 인스펙터에서 'Story Text' 및 'Story Panel'을 직접 연결했는지 확인하세요.");
            return;
        }

        storyLines = newLines;

        // 초기화 및 UI 활성화
        StopAllCoroutines();
        isTyping = false;
        currentLineIndex = 0;
        isStoryActive = true;

        // UI 활성화
        if (titlePanel != null) titlePanel.SetActive(false);
        storyPanel.SetActive(true); // storyPanel은 null 체크 통과했으므로 바로 사용
        if (portraitPanelObject != null) portraitPanelObject.SetActive(true);

        StartCoroutine(TypeStoryLine());
    }

    // 타이핑 효과 코루틴
    IEnumerator TypeStoryLine()
    {
        isTyping = true;
        storyText.text = "";

        string line = storyLines[currentLineIndex];

        for (int i = 0; i < line.Length; i++)
        {
            storyText.text += line[i];
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    // 플레이어가 Spacebar를 눌렀을 때 호출되어 대화를 진행합니다.
    public void NextLine()
    {
        if (!isStoryActive || storyText == null) return;

        if (isTyping)
        {
            // 타이핑 중일 때: 즉시 전체 텍스트 출력
            StopAllCoroutines();
            storyText.text = storyLines[currentLineIndex];
            isTyping = false;
            return;
        }

        currentLineIndex++;

        if (currentLineIndex < storyLines.Length)
        {
            StartCoroutine(TypeStoryLine());
        }
        else
        {
            // 대화의 마지막 줄을 봤다면 정리 함수 호출
            EndStory();
        }
    }

    // 대화 종료 및 정리 로직
    void EndStory()
    {
        isStoryActive = false;

        // (타이틀/스토리 패널 정리)
        if (storyPanel != null) storyPanel.SetActive(false);
        if (titlePanel != null) titlePanel.SetActive(true);

        // NPC의 말풍선 및 일러스트 닫기 (Null 체크)
        if (portraitDisplayImage != null)
        {
            portraitDisplayImage.sprite = null;
        }
        if (portraitPanelObject != null)
        {
            portraitPanelObject.SetActive(false);
        }

        if (activeSpeechBubble != null)
        {
            activeSpeechBubble.SetActive(false);
            activeSpeechBubble = null;
        }

        if (storyText != null) storyText.text = "";
    }
}