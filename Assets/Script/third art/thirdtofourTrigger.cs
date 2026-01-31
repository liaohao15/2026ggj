using UnityEngine;
using UnityEngine.SceneManagement;

// 类名改为 thirdtofourTrigger（建议首字母大写更符合C#规范，这里按你的要求来）
public class thirdtofourTrigger : MonoBehaviour
{
    // 可在编辑器直接修改目标场景名称，更灵活
    [Header("切换目标场景")]
    public string targetSceneName = "fourth act";

    // 当玩家触发时切换场景
    private void OnTriggerEnter(Collider other)
    {
        // 验证触发者是玩家（标签需为"Player"）
        if (other.CompareTag("Player"))
        {
            // 加载指定名称的场景
            SceneManager.LoadScene(6);

            // 如果仍想用索引，替换上面一行即可：
            // SceneManager.LoadScene(3); // 前提是fourth act的索引为3
        }
    }
}