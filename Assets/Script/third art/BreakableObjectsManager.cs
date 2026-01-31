using UnityEngine;

public class BreakableObjectsManager : MonoBehaviour
{
    [Header("配置")]
    public GameObject[] breakableObjects; // 需要破坏的物体列表
    public DialogManager dialogManager;   // 对话管理器
    public GameObject door;               // 门物体

    private int brokenCount = 0;          // 已破坏的物体数量
    private bool isTriggered = false;     // 是否已经触发了初始提示

    // 碰撞触发初始提示
    private void OnTriggerEnter(Collider other)
    {
        // 只响应玩家触发
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            // 显示破坏物体的提示
            dialogManager.ShowDialog("需要破坏场景中的 " + breakableObjects.Length + " 个物体来打开门！", 5f);

            // 给每个可破坏物体添加监听
            foreach (GameObject obj in breakableObjects)
            {
                BreakableObject breakable = obj.GetComponent<BreakableObject>();
                if (breakable == null)
                {
                    breakable = obj.AddComponent<BreakableObject>();
                }
                breakable.manager = this;
            }
        }
    }

    // 物体被破坏时调用
    public void OnObjectBroken()
    {
        brokenCount++;

        // 检查是否全部破坏
        if (brokenCount >= breakableObjects.Length)
        {
            // 显示门已打开提示
            dialogManager.ShowDialog("门已经打开了！", 3f);

            // 开门（简单处理：隐藏门物体）
            if (door != null)
            {
                door.SetActive(false);
            }
        }
        else
        {
            // 显示剩余数量提示
            dialogManager.ShowDialog("还需要破坏 " + (breakableObjects.Length - brokenCount) + " 个物体！", 2f);
        }
    }
}