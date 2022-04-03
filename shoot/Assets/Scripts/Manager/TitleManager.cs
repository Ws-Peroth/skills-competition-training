using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Text startText;

    public void OnStartButton()
    {
        SceneManager.LoadScene(1);
    }
    void Start()
    {
        StartCoroutine(AlphaEffect());
    }

    private IEnumerator AlphaEffect()
    {
        while (true)
        {
            EffectManager.Instance.FadeOutEffect(startText, 2f);
            yield return new WaitForSeconds(2f);
            EffectManager.Instance.FadeInEffect(startText, 2f);
            yield return new WaitForSeconds(2f);
        }
    }
}
