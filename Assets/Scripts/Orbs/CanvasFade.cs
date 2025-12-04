using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage; // 전체 화면 검은 이미지
    public float fadeDuration = 2f;

    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(0f, 1f));
    }

    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(1f, 0f));
    }

    IEnumerator FadeRoutine(float from, float to)
    {
        float elapsed = 0f;
        Color c = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            float a = Mathf.Lerp(from, to, t);

            c.a = a;
            fadeImage.color = c;

            yield return null;
        }

        c.a = to;
        fadeImage.color = c;
    }
}
