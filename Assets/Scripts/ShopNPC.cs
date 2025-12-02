using UnityEngine;
using System.Collections; // 코루틴을 위해 필요

public class ShopNPC : MonoBehaviour
{
    [TextArea(3, 5)]
    public string[] sayLines;

    private GameIntroManager dialogueManager;

    private int currentDialogueIndex = 0;

    [Header("말풍선 설정")]
    public GameObject speechBubbleObject;
    public float hideDelay = 3.0f;


    void Start()
    {
        dialogueManager = FindFirstObjectByType<GameIntroManager>();

        // 시작 시 말풍선 숨기기 
        if (speechBubbleObject != null)
        {
            speechBubbleObject.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        // 씬 로드 에러 방지 및 대사가 있을 때만 실행
        if (dialogueManager == null || sayLines.Length == 0) return;


        // 말풍선 표시 및 타이머 시작 로직 
        if (speechBubbleObject != null)
        {
            // 말풍선 켜기
            speechBubbleObject.SetActive(true);

            // 이전에 실행 중인 '숨기기' 코루틴이 있다면 멈추고 새로 시작
            StopCoroutine("HideSpeechBubbleAfterDelay");
            StartCoroutine(HideSpeechBubbleAfterDelay(hideDelay));
        }


        // 대화 순환 및 출력 로직 

        // 현재 순서의 대사 한 줄을 가져옴 
        string currentLine = sayLines[currentDialogueIndex];

        // 매니저에게 '새로운 대사 배열'로 포장해서 전달
        dialogueManager.ShowDialogue(new string[] { currentLine });

        // 순서를 다음으로 넘김
        currentDialogueIndex++;

        // 대사 목록의 끝에 도달하면 0으로 리셋하여 처음부터 다시 순환
        if (currentDialogueIndex >= sayLines.Length)
        {
            currentDialogueIndex = 0;
            
        }
    }


    // 클래스 안에 올바르게 위치한 코루틴 함수 
    IEnumerator HideSpeechBubbleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (speechBubbleObject != null)
        {
            speechBubbleObject.SetActive(false);
        }
    }
}