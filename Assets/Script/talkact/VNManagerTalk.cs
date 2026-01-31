using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VNManagerTalk : MonoBehaviour
{
    // 对齐原有UI命名
    public TextMeshProUGUI SpeakerName;
    public TextMeshProUGUI SpeekContent;
    public TypewriterEffectTalk typewriterEffect;
    public Image AvatarImage;
    public AudioSource VocalAudio;

    // 按钮名改为TalktoMenu
    public Button TalktoMenu;
    // 目标场景：索引8（或填Menu场景名）
    public int targetSceneIndex = 8;

    // 自动读取ConstantsTalk里的4.xlsx，无需改
    private string storyPath = ConstantsTalk.STORY_PATH;
    private string defaultStoryFileName = ConstantsTalk.DEFAULT_STORY_FILE_NAME;
    private List<ExcelReaderTalk.ExcelDataTalk> storyData;
    private int currentLine = 0;

    void Start()
    {
        // 初始化TalktoMenu按钮
        if (TalktoMenu != null)
        {
            TalktoMenu.gameObject.SetActive(false);
            TalktoMenu.onClick.AddListener(SwitchToMenuScene);
        }

        // 自动加载4.xlsx
        LoadStoryFromFile(storyPath + defaultStoryFileName);
        if (storyData != null && storyData.Count > 0)
        {
            DisplayNextLine();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DisplayNextLine();
        }
    }

    void LoadStoryFromFile(string path)
    {
        storyData = ExcelReaderTalk.ReadExcel(path);
        if (storyData == null || storyData.Count == 0)
        {
            Debug.LogError(ConstantsTalk.NO_DATA_FOUND);
        }
    }

    void DisplayNextLine()
    {
        if (storyData == null || currentLine >= storyData.Count)
        {
            Debug.Log(ConstantsTalk.END_OF_STORY);
            if (TalktoMenu != null)
            {
                TalktoMenu.gameObject.SetActive(true);
            }
            return;
        }

        if (typewriterEffect.IsTyping())
        {
            typewriterEffect.CompleteLine();
        }
        else
        {
            if (TalktoMenu != null)
            {
                TalktoMenu.gameObject.SetActive(false);
            }
            DisplayThisLine();
        }
    }

    void DisplayThisLine()
    {
        var data = storyData[currentLine];
        SpeakerName.text = data.speakerName;
        SpeekContent.text = data.speakingContent;
        typewriterEffect.StartTyping(SpeekContent.text);

        if (!string.IsNullOrEmpty(data.avatarImageFileName))
        {
            UpdateAvatarImage(data.avatarImageFileName);
        }
        else
        {
            AvatarImage.gameObject.SetActive(false);
        }

        if (!string.IsNullOrEmpty(data.vocalAudioFileName))
        {
            PlayVocalAudio(data.vocalAudioFileName);
        }

        currentLine++;
        StartCoroutine(WaitForContentComplete());
    }

    void UpdateAvatarImage(string imageFileName)
    {
        string imagePath = ConstantsTalk.AVATAR_PATH + imageFileName;
        Sprite sprite = Resources.Load<Sprite>(imagePath);
        if (sprite != null)
        {
            AvatarImage.sprite = sprite;
            AvatarImage.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError(ConstantsTalk.IMAGE_LOAD_FAILED + imagePath);
        }
    }

    void PlayVocalAudio(string audioFileName)
    {
        string audioPath = "Talk/audio/vocal/1";
        AudioClip audioClip = Resources.Load<AudioClip>(audioPath);
        if (audioClip == null)
        {
            audioClip = Resources.Load<AudioClip>(audioPath + ".mp3");
        }

        if (audioClip != null)
        {
            VocalAudio.Stop();
            VocalAudio.clip = audioClip;
            VocalAudio.Play();
            Debug.Log("代码成功播放：1.mp3");
        }
        else
        {
            Debug.LogError(ConstantsTalk.AUDIO_LOAD_FAILED + audioPath);
        }
    }

    IEnumerator WaitForContentComplete()
    {
        while (typewriterEffect.IsTyping())
        {
            yield return null;
        }

        if (VocalAudio.isPlaying)
        {
            yield return new WaitUntil(() => !VocalAudio.isPlaying);
        }

        if (currentLine >= storyData.Count && TalktoMenu != null)
        {
            TalktoMenu.gameObject.SetActive(true);
        }
    }

    // 切换到8号场景
    void SwitchToMenuScene()
    {
        try
        {
            SceneManager.LoadScene(0);
            Debug.Log("成功切换到场景索引：" + 0);
        }
        catch (System.Exception e)
        {
            Debug.LogError("切换场景失败：" + e.Message);
        }
    }
}