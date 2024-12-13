using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointManager : MonoBehaviour
{
    public List<GameObject> checkPoints = new List<GameObject>();
    private GameObject player;
    private GameObject currentCheckPoint;
    private GameObject restartPosition;
    public float fadeDuration = 2f;
    public Image fadeImage;
    public Image gameOver;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        GameObject startCheckPoint = new GameObject("StartCheckPoint");
        startCheckPoint.transform.position = player.transform.position;

        UpdateCheckPoint(startCheckPoint);
        restartPosition = startCheckPoint;
    }

    public void Respawn()
    {
        StartCoroutine(FadeOut());
    }

    public void UpdateCheckPoint(GameObject newCheckPoint)
    {
        if (currentCheckPoint != null && currentCheckPoint == newCheckPoint)
        {
            return;
        }
        currentCheckPoint = newCheckPoint; 
        if (!checkPoints.Contains(newCheckPoint))
        {
            checkPoints.Add(newCheckPoint);
        }
    }

    public void ReStart()
    {
        StartCoroutine(GameOverFadeOut());
    }

    private IEnumerator FadeOut()
    {
        float timeElapsed = 0f;
        Color startColor = fadeImage.color;
        while (timeElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(startColor.a, 1f, timeElapsed / fadeDuration);
            fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, 1f);
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(fadeDuration);

        if (currentCheckPoint != null)
        {
            player.transform.position = currentCheckPoint.transform.position;
        }
        else
        {
            player.transform.position = restartPosition.transform.position;
        }
        float timeElapsed = 0f;
        Color startColor = fadeImage.color;
        while (timeElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(startColor.a, 0f, timeElapsed / fadeDuration);
            fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
    }
    private IEnumerator GameOverFadeOut()
    {
        float timeElapsed = 0f;
        gameOver.enabled = true;
        Color startColor = gameOver.color;
        while (timeElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(startColor.a, 1f, timeElapsed / fadeDuration);
            gameOver.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        gameOver.color = new Color(startColor.r, startColor.g, startColor.b, 1f);
        if (currentCheckPoint != null)
        {
            player.transform.position = currentCheckPoint.transform.position;
        }
        else
        {
            player.transform.position = restartPosition.transform.position;
        }
    }
}