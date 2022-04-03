using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void FadeInEffect(Graphic fadeObject, float time)
    {
        StartCoroutine(FadeEffect(fadeObject, time, true));
    }

    public void FadeOutEffect(Graphic fadeObject, float time)
    {
        StartCoroutine(FadeEffect(fadeObject, time, false));
    }
    
    private IEnumerator FadeEffect(Graphic fadeObject, float time, bool isFadeIn)
    {
        fadeObject.gameObject.SetActive(true);
        fadeObject.color = new Color(fadeObject.color.r, fadeObject.color.g, fadeObject.color.b, isFadeIn ? 0f : 1f);
        var loopCount = time / 0.01f;
        var deltaValue = time / loopCount;

        for (var i = loopCount; i > 0; i--)
        {
            if (fadeObject == null)
            {
                yield break;
            }
            Debug.Log($"{i}");
            var alpha = fadeObject.color.a + (isFadeIn ? deltaValue : -deltaValue);
            fadeObject.color = new Color(fadeObject.color.r, fadeObject.color.g, fadeObject.color.b, alpha);
            yield return new WaitForSeconds(0.01f);
        }

        Debug.Log(isFadeIn ? "Fade In Finish" : "Fade Out Finish");
    }
}
