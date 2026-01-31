using UnityEngine;
using TMPro; // 需要导入TextMeshPro包

public class DialogManager : MonoBehaviour
{
    [Header("UI设置")]
    public TextMeshProUGUI dialogText; // 场景中创建的TextMeshPro文本
    public GameObject dialogPanel;     // 显示提示的面板

    private void Start()
    {
        // 初始隐藏提示面板
        if (dialogPanel != null)
        {
            dialogPanel.SetActive(false);
        }
    }

    // 显示提示文本
    public void ShowDialog(string text, float showTime = 0)
    {
        if (dialogPanel != null && dialogText != null)
        {
            dialogPanel.SetActive(true);
            dialogText.text = text;

            // 如果showTime>0，自动隐藏
            if (showTime > 0)
            {
                Invoke("HideDialog", showTime);
            }
        }
    }

    // 隐藏提示面板
    public void HideDialog()
    {
        if (dialogPanel != null)
        {
            dialogPanel.SetActive(false);
        }
    }
}