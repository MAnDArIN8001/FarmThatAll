using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeDuration = 1f;

    public void Start()
    {
        StartCoroutine(FadeFromBlack());
    }
    public void FadeAndLoadScene(string sceneName)
    {
        _fadeImage.gameObject.SetActive(true);

        StartCoroutine(FadeOutAndLoad(sceneName));
    }
    private IEnumerator FadeFromBlack()
    {
        float t = 0f;
        Color color = _fadeImage.color;
        color.a = 1f;
        _fadeImage.color = color;

        while (t < _fadeDuration)
        {
            t += Time.deltaTime;
            color.a = 1f - t / _fadeDuration;
            _fadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        _fadeImage.color = color;
        _fadeImage.gameObject.SetActive(false);
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        float t = 0f;
        Color color = _fadeImage.color;
        color.a = 0f;
        _fadeImage.color = color;

        while (t < _fadeDuration)
        {
            t += Time.deltaTime;
            color.a = t / _fadeDuration;
            _fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}
