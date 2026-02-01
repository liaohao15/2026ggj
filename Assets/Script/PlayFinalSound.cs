using UnityEngine;
public class PlayFinalSound : MonoBehaviour
{
    void Start()
    {
        // 场景加载后自动切换到最后场景的音效
        SoundManager.Instance.SwitchToFinalSound();
    }
}