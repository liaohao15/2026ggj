using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffectFive : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public float waitingSeconds = ConstantsFive.DEFAULT_WAITING_SECONDS; // ¸ÄConstantsFive

    private Coroutine typingCoroutine;
    private bool isTyping;

    public void StartTyping(string text)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeLine(text));
    }

    private IEnumerator TypeLine(string text)
    {
        isTyping = true;
        textDisplay.text = text;
        textDisplay.maxVisibleCharacters = 0;

        for (int i = 0; i <= text.Length; i++)
        {
            textDisplay.maxVisibleCharacters = i;
            yield return new WaitForSeconds(waitingSeconds);
        }

        isTyping = false;
    }

    public void CompleteLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        textDisplay.maxVisibleCharacters = textDisplay.text.Length;
        isTyping = false;
    }

    public bool IsTyping()
    {
        return isTyping;
    }
}