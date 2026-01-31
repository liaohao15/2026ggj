using UnityEngine;
using TMPro; // 新增：引用TextMeshPro命名空间

public class TaskTextInit : MonoBehaviour
{
    [Header("任务1配置")]
    public TextMeshProUGUI task1Text; // 改为TextMeshProUGUI
    public string task1InitText = "1. 寻找球体";
    [Header("任务2配置")]
    public TextMeshProUGUI task2Text; // 改为TextMeshProUGUI
    public string task2InitText = "2. 寻找立方体";

    void Start()
    {
        if (task1Text != null) task1Text.text = task1InitText;
        if (task2Text != null) task2Text.text = task2InitText;
    }
}