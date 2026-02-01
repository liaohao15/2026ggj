using UnityEngine;
using TMPro; // 新增：引用TextMeshPro命名空间

public class TaskTextInit : MonoBehaviour
{
    [Header("任务1配置")]
    public TextMeshProUGUI task1Text; // 改为TextMeshProUGUI
    public string task1InitText = "寻找挂号单";
    [Header("任务2配置")]
    public TextMeshProUGUI task2Text; // 改为TextMeshProUGUI
    public string task2InitText = "寻找平安扣";
    [Header("任务2配置")]
    public TextMeshProUGUI task3Text; // 改为TextMeshProUGUI
    public string task3InitText = "寻找吊水瓶";

    void Start()
    {
        if (task1Text != null) task1Text.text = task1InitText;
        if (task2Text != null) task2Text.text = task2InitText;
        if (task3Text != null) task3Text.text = task3InitText;
    }
}