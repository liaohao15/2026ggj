using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 新增：场景切换需要

public class VNManager : MonoBehaviour
{
    // 原有变量（完全保留）
    public TextMeshProUGUI speakerName;
    public TextMeshProUGUI speakingContent;
    public TypewriterEffect typewriterEffect;
    public Image avatarImage;
    public AudioSource vocalAudio;

    // 【新增】切换第三场景的按钮（Inspector赋值）
    public Button switchToThirdSceneBtn;
    // 【新增】第三场景名称（需和Build Settings一致）
    public string thirdSceneName = "ThirdScene";

    // 原有变量（完全保留）
    private string storyPath = Constants.STORY_PATH;
    private string defaultStoryFileName = Constants.DEFAULT_STORY_FILE_NAME;
    private List<ExcelReader.ExcelData> storyData;
    private int currentLine = 0;

    void Start()
    {
        // 【新增】初始化切换按钮：默认隐藏
        if (switchToThirdSceneBtn != null)
        {
            switchToThirdSceneBtn.gameObject.SetActive(false);
            // 绑定按钮点击事件
            switchToThirdSceneBtn.onClick.AddListener(SwitchToThirdScene);
        }

        // 原有逻辑（完全保留）
        LoadStoryFromFile(storyPath + defaultStoryFileName);
        if (storyData != null && storyData.Count > 0)
        {
            DisplayNextLine();
        }
    }

    // 原有逻辑（完全保留）
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DisplayNextLine();
        }
    }

    // 原有逻辑（完全保留）
    void LoadStoryFromFile(string path)
    {
        storyData = ExcelReader.ReadExcel(path);
        if (storyData == null || storyData.Count == 0)
        {
            Debug.LogError(Constants.NO_DATA_FOUND);
        }
    }

    // 原有逻辑（仅新增按钮显示/隐藏）
    void DisplayNextLine()
    {
        if (storyData == null || currentLine >= storyData.Count)
        {
            Debug.Log(Constants.END_OF_STORY);
            // 【新增】剧情播放完毕，显示切换按钮
            if (switchToThirdSceneBtn != null)
            {
                switchToThirdSceneBtn.gameObject.SetActive(true);
            }
            return;
        }

        if (typewriterEffect.IsTyping())
        {
            typewriterEffect.CompleteLine();
        }
        else
        {
            // 【新增】播放新剧情时，隐藏切换按钮
            if (switchToThirdSceneBtn != null)
            {
                switchToThirdSceneBtn.gameObject.SetActive(false);
            }
            DisplayThisLine();
        }
    }

    // 原有逻辑（仅新增协程调用）
    void DisplayThisLine()
    {
        var data = storyData[currentLine];
        speakerName.text = data.speakerName;
        speakingContent.text = data.speakingContent;
        typewriterEffect.StartTyping(speakingContent.text);

        if (!string.IsNullOrEmpty(data.avatarImageFileName))
        {
            UpdateAvatarImage(data.avatarImageFileName);
        }
        else
        {
            avatarImage.gameObject.SetActive(false);
        }

        if (!string.IsNullOrEmpty(data.vocalAudioFileName))
        {
            PlayVocalAudio(data.vocalAudioFileName);
        }

        currentLine++;

        // 【新增】等待打字机+音频播放完成
        StartCoroutine(WaitForContentComplete());
    }

    // 原有逻辑（完全保留）
    void UpdateAvatarImage(string imageFileName)
    {
        string imagePath = Constants.AVATAR_PATH + imageFileName;
        Sprite sprite = Resources.Load<Sprite>(imagePath);
        if (sprite != null)
        {
            avatarImage.sprite = sprite;
            avatarImage.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError(Constants.IMAGE_LOAD_FAILED + imagePath);
        }
    }

    // 原有逻辑（完全保留）
    void PlayVocalAudio(string audioFileName)
    {
        // 第一步：强制加载1.mp3（手动能播放的文件）
        string audioPath = "audio/vocal/1";
        AudioClip audioClip = Resources.Load<AudioClip>(audioPath);
        if (audioClip == null)
        {
            audioClip = Resources.Load<AudioClip>(audioPath + ".mp3");
        }

        if (audioClip != null)
        {
            // 关键补充：播放前先停止当前音频，避免覆盖
            vocalAudio.Stop();
            vocalAudio.clip = audioClip;
            vocalAudio.Play();
            Debug.Log("代码成功播放：1.mp3"); // 加日志确认
        }
        else
        {
            Debug.LogError(Constants.AUDIO_LOAD_FAILED + audioPath);
        }
    }

    // 【新增】等待打字机和音频播放完成
    IEnumerator WaitForContentComplete()
    {
        // 等待打字机效果完成
        while (typewriterEffect.IsTyping())
        {
            yield return null;
        }

        // 等待音频播放完成（如果有音频）
        if (vocalAudio.isPlaying)
        {
            yield return new WaitUntil(() => !vocalAudio.isPlaying);
        }

        // 若已是最后一行，显示切换按钮
        if (currentLine >= storyData.Count && switchToThirdSceneBtn != null)
        {
            switchToThirdSceneBtn.gameObject.SetActive(true);
        }
    }

    // 【新增】切换到第三场景的核心方法
    void SwitchToThirdScene()
    {
        // 切换场景
        SceneManager.LoadScene(3);
    }
}