using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;


public class Comic : MonoBehaviour
{
    [SerializeField] private List<Sprite> _slides;

    [SerializeField] private Image _slideImage;
    [SerializeField] private Image _fadeImage;

    [SerializeField] private float _fadeDuration;
    [SerializeField] private float _slideDelay;
    
    [SerializeField] private StartMenu _startMenu;

    void OnEnable()
    {
        StartCoroutine(SlideShow());
    }
    
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            NextSlide();
        }
    }

    private void NextSlide()
    {
        if (_slides.Count > 0)
        {
            _slideImage.sprite = _slides.First();

            _slides.RemoveAt(0);
        }
        else
        {
            _startMenu.FadeAndLoad();
        }
    }
    
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    
    private IEnumerator SlideShow()
    {
        while (_slides.Count >= 0)
        {
            yield return StartCoroutine(Fade(0f, 1f));

            NextSlide();

            if (this == null)
                yield break;

            yield return StartCoroutine(Fade(1f, 0f));
            yield return new WaitForSeconds(_slideDelay);
        }
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float t = 0f;
        Color color = _fadeImage.color;
        color.a = startAlpha;
        _fadeImage.color = color;

        while (t < _fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(startAlpha, endAlpha, t / _fadeDuration);
            color.a = a;
            _fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        _fadeImage.color = color;
    }


}
