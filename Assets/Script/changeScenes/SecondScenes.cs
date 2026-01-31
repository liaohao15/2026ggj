using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTrigger : MonoBehaviour
{
    // 当人物碰到这个触发物体时，自动切换到第三个场景（索引为2）
    private void OnTriggerEnter(Collider other)
    {
        // 确保是玩家触发的（你的人物需要有"Player"标签）
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(2);
        }
    }
}