using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    // 全局主背景音乐（全程播放）
    public AudioClip mainMusic;
    // 最后场景的音效
    public AudioClip finalSceneSound;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            // 主音乐循环播放，音量调大（覆盖UI音乐）
            audioSource.loop = true;
            audioSource.volume = 1.0f;
            audioSource.clip = mainMusic;
            audioSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 切换到最后场景的音效（仅最后场景调用一次）
    public void SwitchToFinalSound()
    {
        audioSource.Stop();
        audioSource.clip = finalSceneSound;
        audioSource.loop = true; // 不需要循环就设为 false
        audioSource.Play();
    }

    // 调整主音乐音量（用来强化覆盖效果）
    public void SetMainVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume);
    }
}