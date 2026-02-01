using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void ExitGame()
    {
        // 退出游戏
        Application.Quit();

        // 编辑器内停止运行
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}