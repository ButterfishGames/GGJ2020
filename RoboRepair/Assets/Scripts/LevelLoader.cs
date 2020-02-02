using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader singleton;

    public float fadeTime;

    private float inverseFadeTime;

    private Image fader;

    private void Start()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        fader = GetComponentInChildren<Image>();

        inverseFadeTime = 1.0f / fadeTime;

        StartCoroutine(LevelFade(true));
    }

    public void LoadScene(int buildIndex)
    {
        StartCoroutine(FadeAndLoad(buildIndex));
    }

    private IEnumerator FadeAndLoad(int buildIndex)
    {
        StartCoroutine(LevelFade(false));
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(buildIndex);
        StartCoroutine(LevelFade(true));
    }

    private IEnumerator LevelFade(bool fadeIn)
    {
        Color temp = fader.color;
        if (fadeIn)
        {
            temp.a = 1;
            fader.color = temp;
            while (fader.color.a > 0)
            {
                temp.a -= inverseFadeTime * Time.deltaTime;
                fader.color = temp;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            temp.a = 0;
            fader.color = temp;
            while (fader.color.a < 1)
            {
                temp.a += inverseFadeTime * Time.deltaTime;
                fader.color = temp;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
