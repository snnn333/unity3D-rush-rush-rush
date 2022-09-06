using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public Texture2D FadingTexture;
    public float FadeTime = 1;

    private int drawDepth = -1000;
    private float _alpha = 1;
    private int _fadeDirection = -1;


    public void LoadNewScene(string sceneName)
    {
        if (Math.Abs(Time.timeScale) < 0.1)
        {
            Time.timeScale = 1;
        }

        StartCoroutine(FadeScene(sceneName));
    }

    private void OnGUI()
    {
        _alpha += _fadeDirection * FadeTime * Time.deltaTime;
        _alpha = Mathf.Clamp01(_alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, _alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), FadingTexture);
    }

    private void BeginFade(int direction)
    {
        _fadeDirection = direction;
    }
    
    IEnumerator FadeScene(string sceneName)
    {
        BeginFade(1);
        yield return new WaitForSeconds(FadeTime);
        SceneManager.LoadScene(sceneName);
    }
}