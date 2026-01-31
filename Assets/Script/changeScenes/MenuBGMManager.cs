using UnityEngine;
using UnityEngine.SceneManagement;
// 必须添加这个命名空间，否则识别不了IEnumerator
using System.Collections;

public class MenuBGMManager : MonoBehaviour
{
    // 单例实例，确保全局只有一个BGM播放源
    public static MenuBGMManager instance;

    // 音频组件
    private AudioSource audioSource;

    // 淡入/淡出的时长（秒），可在Inspector面板调整
    [Header("音效过渡设置")]
    public float fadeInDuration = 1.5f;   // 淡入时长
    public float fadeOutDuration = 1f;    // 淡出时长
    public float targetVolume = 0.6f;     // 目标音量（0-1）

    void Awake()
    {
        // 单例逻辑：确保只有一个BGM对象
        if (instance == null)
        {
            instance = this;
            // 切换场景时不销毁，直到手动处理
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();

            // 初始化音量为0，准备淡入
            audioSource.volume = 0;
            audioSource.loop = true; // 循环播放
            audioSource.playOnAwake = false; // 禁止自动播放，由脚本控制
        }
        else
        {
            // 如果已有实例，销毁重复的
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 开始播放并执行淡入
        audioSource.Play();
        StartCoroutine(FadeInBGM());
    }

    // 音效淡入协程（正确的IEnumerator类型）
    IEnumerator FadeInBGM()
    {
        float currentTime = 0;
        float startVolume = 0;

        // 逐步提升音量
        while (currentTime < fadeInDuration)
        {
            currentTime += Time.deltaTime;
            // 线性插值计算当前音量
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / fadeInDuration);
            yield return null; // 等待下一帧
        }

        // 确保最终音量准确
        audioSource.volume = targetVolume;
    }

    // 音效淡出协程（供场景切换时调用）
    IEnumerator FadeOutAndStop()
    {
        float currentTime = 0;
        float startVolume = audioSource.volume;

        // 逐步降低音量
        while (currentTime < fadeOutDuration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, currentTime / fadeOutDuration);
            yield return null;
        }

        // 淡出完成后停止播放并销毁对象
        audioSource.Stop();
        Destroy(gameObject);
    }

    // 监听场景加载事件
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 场景加载完成后执行的逻辑
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 替换成你的游戏关卡场景名称（比如"GameLevel1"）
        if (scene.name == "first act")
        {
            // 开始淡出并停止BGM
            StartCoroutine(FadeOutAndStop());
        }
    }
}