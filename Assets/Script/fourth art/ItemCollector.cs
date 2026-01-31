using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ItemCollector : MonoBehaviour
{
    [Header("玩家UI脚本引用")]
    public PlayerUIInput playerUI;
    [Header("对应任务文本（TextMeshPro）")]
    public TextMeshProUGUI targetTaskText;
    [Header("原任务文本")]
    public string originalTaskText; // 例如："1. 寻找钥匙"
    [Header("最后一个物品：切换场景名")]
    public string nextSceneName;

    // 全局计数器：已收集物品数量（无顺序限制）
    private static int collectedItemCount = 0;
    // 场景中可收集物品总数（改这里！有几个填几个）
    public static int totalItemCount = 2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 防止重复收集（比如物品没销毁，重复触发）
            if (gameObject.activeSelf == false) return;

            // 非顺序更新UI：找到哪个更哪个，绿色“已找到”
            if (targetTaskText != null)
            {
                targetTaskText.text = originalTaskText + " <color=green>已找到</color>";
                Debug.Log("Item [" + gameObject.name + "] collected successfully");
            }

            gameObject.SetActive(false);
            collectedItemCount++; // 收集一个，计数+1（不管顺序）

            // 只有收集完所有物品，才切换场景（无顺序要求）
            if (collectedItemCount >= totalItemCount && !string.IsNullOrEmpty(nextSceneName))
            {
                Invoke("SwitchToStoryScene", 1f);
            }
        }
    }

    void SwitchToStoryScene()
    {
        collectedItemCount = 0; // 重置计数，不影响其他场景
        SceneManager.LoadScene(nextSceneName);
    }

    // 可选：游戏重启时重置计数（防止场景切换后计数残留）
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        collectedItemCount = 0;
    }
}