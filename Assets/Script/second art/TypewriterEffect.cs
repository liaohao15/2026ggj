using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    // 要显示文字的TMP文本组件（在Inspector中赋值）
    public TextMeshProUGUI textDisplay;

    // 打字间隔时间（默认从Constants读取）
    public float waitingSeconds = Constants.DEFAULT_WAITING_SECONDS;

    private Coroutine typingCoroutine;
    private bool isTyping;

    /// <summary>
    /// 开始打字机效果
    /// </summary>
    /// <param name="text">要显示的完整文本</param>
    public void StartTyping(string text)
    {
        // 如果当前正在打字，先中断旧的协程
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeLine(text));
    }

    /// <summary>
    /// 逐字显示文本的协程
    /// </summary>
    private IEnumerator TypeLine(string text)
    {
        isTyping = true;
        textDisplay.text = text;
        textDisplay.maxVisibleCharacters = 0;

        // 逐字显示（注意：i <= text.Length 确保最后一个字也能显示）
        for (int i = 0; i <= text.Length; i++)
        {
            textDisplay.maxVisibleCharacters = i;
            yield return new WaitForSeconds(waitingSeconds);
        }

        isTyping = false;
    }

    /// <summary>
    /// 跳过打字，直接显示完整文本
    /// </summary>
    public void CompleteLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        textDisplay.maxVisibleCharacters = textDisplay.text.Length;
        isTyping = false;
    }

    /// <summary>
    /// 是否正在打字
    /// </summary>
    public bool IsTyping()
    {
        return isTyping;
    }
}