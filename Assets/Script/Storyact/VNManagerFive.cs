using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VNManagerFive : MonoBehaviour
{
    // 完全对齐你的旧场景UI命名
    public TextMeshProUGUI SpeakerName;
    public TextMeshProUGUI SpeekContent;
    public TypewriterEffectFive typewriterEffect;
    public Image AvatarImage;
    public AudioSource VocalAudio;

    // 按钮名改为你的storytolast
    public Button storytolast;
    // 目标场景名改为Menu
    public string targetSceneName = "Menu";

    private string storyPath = ConstantsFive.STORY_PATH;
    private string defaultStoryFileName = ConstantsFive.DEFAULT_STORY_FILE_NAME;
    private List<ExcelReaderFive.ExcelDataFive> storyData;
    private int currentLine = 0;

    void Start()
    {
        // 初始化storytolast按钮
        if (storytolast != null)
        {
            storytolast.gameObject.SetActive(false);
            storytolast.onClick.AddListener(SwitchToMenuScene);
        }

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
        storyData = ExcelReaderFive.ReadExcel(path);
        if (storyData == null || storyData.Count == 0)
        {
            Debug.LogError(ConstantsFive.NO_DATA_FOUND);
        }
    }

    void DisplayNextLine()
    {
        if (storyData == null || currentLine >= storyData.Count)
        {
            Debug.Log(ConstantsFive.END_OF_STORY);
            // 剧情结束显示storytolast按钮
            if (storytolast != null)
            {
                storytolast.gameObject.SetActive(true);
            }
            return;
        }

        if (typewriterEffect.IsTyping())
        {
            typewriterEffect.CompleteLine();
        }
        else
        {
            if (storytolast != null)
            {
                storytolast.gameObject.SetActive(false);
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
        string imagePath = ConstantsFive.AVATAR_PATH + imageFileName;
        Sprite sprite = Resources.Load<Sprite>(imagePath);
        if (sprite != null)
        {
            AvatarImage.sprite = sprite;
            AvatarImage.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError(ConstantsFive.IMAGE_LOAD_FAILED + imagePath);
        }
    }

    void PlayVocalAudio(string audioFileName)
    {
        string audioPath = "Five/audio/vocal/1";
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
            Debug.LogError(ConstantsFive.AUDIO_LOAD_FAILED + audioPath);
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

        if (currentLine >= storyData.Count && storytolast != null)
        {
            storytolast.gameObject.SetActive(true);
        }
    }

    // 切换到Menu场景的方法
    void SwitchToMenuScene()
    {
        SceneManager.LoadScene(8);
    }
}