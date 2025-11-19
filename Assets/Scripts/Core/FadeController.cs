using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    [Header("화면을 덮는 검은 Image")]
    public Image fadeImage;

    [Header("페이드 아웃에 걸리는 시간 (초)")]
    public float fadeDuration = 2f;

    private void Awake()
    {
        if (fadeImage != null)
        {
            // 시작할 때는 항상 투명
            SetAlpha(0f);
        }
    }

    public void StartFadeOut()
    {
        if (fadeImage == null)
        {
            Debug.LogError("FadeController: fadeImage 가 지정되지 않았습니다.");
            return;
        }

        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(t / fadeDuration);
            SetAlpha(a);
            yield return null;
        }

        SetAlpha(1f);
    }

    private void SetAlpha(float alpha)
    {
        Color c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;
    }
}
