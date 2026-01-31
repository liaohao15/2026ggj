using UnityEngine;
using TMPro; // 新增：引用TextMeshPro命名空间

public class PlayerUIInput : MonoBehaviour
{
    public GameObject taskUIPanel;
    private bool isUIOpen = false;

    void Start()
    {
        if (taskUIPanel != null)
        {
            taskUIPanel.SetActive(false);
            isUIOpen = false;
        }
        else
        {
            Debug.LogError("【PlayerUIInput】请拖入TaskUI面板！");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isUIOpen)
        {
            OpenTaskUI();
        }
        else if (Input.GetKeyDown(KeyCode.R) && isUIOpen)
        {
            CloseTaskUI();
        }
    }

    void OpenTaskUI()
    {
        taskUIPanel.SetActive(true);
        isUIOpen = true;
    }

    void CloseTaskUI()
    {
        taskUIPanel.SetActive(false);
        isUIOpen = false;
    }

    // 关键修改：参数改为TextMeshProUGUI
    public void UpdateTaskText(TextMeshProUGUI taskText, string newText)
    {
        if (taskText != null)
        {
            taskText.text = newText;
        }
        else
        {
            Debug.LogError("【PlayerUIInput】任务文本为空！");
        }
    }
}