using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    // 全局主背景音乐（全程播放）
    // Global background music (play throughout the game)
    public string mainMusicName = "LikeAFish"; // 音频文件名（无后缀）| Audio file name (no suffix)
    // 最后场景的音效
    // Final scene sound effect
    public string finalSceneSoundName = "TurnThePage"; // 音频文件名（无后缀）| Audio file name (no suffix)

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            // 主音乐循环播放，音量调大（覆盖UI音乐）
            // Play main music in loop, increase volume (override UI music)
            audioSource.loop = true;
            audioSource.volume = 1.0f;

            // 加载主音乐（导出后兼容）
            // Load main music (compatible after export)
            AudioClip mainMusic = Resources.Load<AudioClip>("Audio/" + mainMusicName);
            if (mainMusic != null)
            {
                audioSource.clip = mainMusic;
                audioSource.Play();
            }
            else
            {
                Debug.LogError("主音乐加载失败，请检查Resources/Audio/" + mainMusicName);
                Debug.LogError("Main music load failed, check Resources/Audio/" + mainMusicName);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 切换到最后场景的音效（仅最后场景调用一次）
    // Switch to final scene sound (call once only in final scene)
    public void SwitchToFinalSound()
    {
        audioSource.Stop();
        // 加载最后场景音效（导出后兼容）
        // Load final scene sound (compatible after export)
        AudioClip finalSceneSound = Resources.Load<AudioClip>("Audio/" + finalSceneSoundName);
        if (finalSceneSound != null)
        {
            audioSource.clip = finalSceneSound;
            audioSource.loop = true; // 不需要循环就设为 false | Set to false if no loop needed
            audioSource.Play();
        }
        else
        {
            Debug.LogError("最后场景音效加载失败，请检查Resources/Audio/" + finalSceneSoundName);
            Debug.LogError("Final scene sound load failed, check Resources/Audio/" + finalSceneSoundName);
        }
    }

    // 调整主音乐音量（用来强化覆盖效果）
    // Adjust main music volume (enhance override effect)
    public void SetMainVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume);
    }
}