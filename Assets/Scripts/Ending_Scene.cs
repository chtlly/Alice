using UnityEngine;
using System.Collections; // 코루틴을 위해 필요
using UnityEngine.SceneManagement; // 씬 이동을 위해 필요

public class Ending_Scene : MonoBehaviour
{
    public RectTransform logoTransform; // 로고의 위치를 제어할 RectTransform
    public float moveDuration = 3f;      // 로고가 이동하는 데 걸리는 시간
    public Vector3 endPosition;         // 로고가 멈출 최종 위치

    void Start()
    {
        // Start 함수에서 코루틴을 시작
        StartCoroutine(RunIntroSequence());
    }

    IEnumerator RunIntroSequence()
    {
        // 로고 이동 시작
        yield return StartCoroutine(MoveLogoSmoothly(logoTransform.localPosition, endPosition, moveDuration));

        // 스태프롤 텍스트 출력 
        // yield return new WaitForSeconds(2f); // 2초간 대기하며 텍스트를 보여줌

        // 인트로 시퀀스 종료 및 메인 씬으로 이동
        yield return new WaitForSeconds(10f); // 총 3초간 더 보여준 후

        SceneManager.LoadScene("Start_0"); // 메인 게임 씬 이름으로 바꿔줘
    }

    // 로고를 시작점에서 끝점까지 부드럽게 이동시키는 함수
    IEnumerator MoveLogoSmoothly(Vector3 start, Vector3 end, float duration)
    {
        float startTime = Time.time;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed = Time.time - startTime;
            float t = elapsed / duration; // 0에서 1 사이의 값

            // 부드럽게 이동하기
            logoTransform.localPosition = Vector3.Lerp(start, end, t);

            yield return null; // 다음 프레임까지 대기
        }
        logoTransform.localPosition = end; // 정확히 최종 위치에 고정
    }
}