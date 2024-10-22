using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackGroundText : MonoBehaviour
{
    [System.Serializable]
    public class TextTrigger
    {
        public Transform targetPosition;
        public Image displayImage;
        public TMP_Text displayText;
        public string message;
        public float triggerDistance = 1.0f;
        public float hideDistance = 2.0f;
        public float fadeOutSpeed = 2.0f;
        public bool imageDisplayed = false;
        public bool isTextComplete = false;
        public Coroutine textCoroutine = null;
    }

    public List<TextTrigger> textTriggers;
    public Transform player;

    void Start()
    {
        foreach (TextTrigger trigger in textTriggers)
        {
            trigger.displayImage.gameObject.SetActive(false);
            if (trigger.displayText != null)
            {
                trigger.displayText.gameObject.SetActive(false);
                SetTextAlpha(trigger.displayText, 0f);
            }
        }
    }

    void Update()
    {
        foreach (TextTrigger trigger in textTriggers)
        {
            if (trigger.targetPosition == null)
            {
                continue;
            }

            float distance = Vector2.Distance(new Vector2(player.position.x, player.position.y), new Vector2(trigger.targetPosition.position.x, trigger.targetPosition.position.y));
            if (!trigger.imageDisplayed && distance <= trigger.triggerDistance)
            {
                ShowImage(trigger);
            }
            else if (trigger.imageDisplayed && distance > trigger.hideDistance)
            {
                HideImage(trigger);
            }
        }
    }

    void ShowImage(TextTrigger trigger)
    {
        trigger.displayImage.gameObject.SetActive(true);
        if (trigger.displayText != null)
        {
            trigger.displayText.gameObject.SetActive(true);
            if (trigger.textCoroutine != null)
            {
                StopCoroutine(trigger.textCoroutine);
            }
            trigger.isTextComplete = false;
            trigger.textCoroutine = StartCoroutine(TypeText(trigger));
        }
        trigger.imageDisplayed = true;
    }

    void HideImage(TextTrigger trigger)
    {
        if (!trigger.isTextComplete)
        {
            return;
        }

        if (trigger.textCoroutine != null)
        {
            StopCoroutine(trigger.textCoroutine);
        }
        StartCoroutine(FadeOutText(trigger));
        trigger.imageDisplayed = false;
    }

    public IEnumerator TypeText(TextTrigger trigger)
    {
        trigger.displayText.text = "";
        SetTextAlpha(trigger.displayText, 1f);

        foreach (char c in trigger.message)
        {
            trigger.displayText.text += c;
            if (c == ' ')
            {
                yield return new WaitForSeconds(0.05f);
            }
            else
            {
                yield return null;
            }
        }
        trigger.isTextComplete = true;

        yield return new WaitForSeconds(2f);

        StartCoroutine(FadeOutText(trigger));
    }

    IEnumerator FadeOutText(TextTrigger trigger)
    {
        TMP_Text text = trigger.displayText;
        float alpha = text.color.a;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * trigger.fadeOutSpeed;
            SetTextAlpha(text, alpha);
            yield return null;
        }

        text.gameObject.SetActive(false);
        trigger.displayImage.gameObject.SetActive(false);
    }

    void SetTextAlpha(TMP_Text text, float alpha)
    {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }
}