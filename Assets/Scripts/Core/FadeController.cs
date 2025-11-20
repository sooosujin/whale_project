using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    [Header("화면을 덮는 검은 Image")]
    public Image fadeImage;

    [Header("페이드 아웃에 걸리는 시간 (초)")]
    public float fadeDuration = 2f;

    // private void Awake()
    // {
    //     if (fadeImage != null)
    //     {
    //         // 시작할 때는 항상 투명
    //         SetAlpha(0f);
    //     }
    // }

    public void StartFadeOut()
    {
        if (fadeImage == null)
        {
            Debug.LogError("FadeController: fadeImage 가 지정되지 않았습니다.");
            return;
        }

        StartCoroutine(FadeOutCoroutine());
    }

    // 🔥 Coroutine을 직접 반환하는 버전 (SimpleRecorder에서 yield return 으로 쓰기 위함)
    public IEnumerator StartFadeOutCoroutine()
    {
        yield return StartCoroutine(FadeOutCoroutine());
    }

    // 🔥 실제 페이드 아웃 코루틴
    private IEnumerator FadeOutCoroutine()
    {
        float elapsed = 0f;
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;

        // 0 → 1까지 천천히 알파 올리기
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);

            c.a = t;
            fadeImage.color = c;

            yield return null;
        }

        // 최종적으로 완전히 검게
        c.a = 1f;
        fadeImage.color = c;
    }
}
