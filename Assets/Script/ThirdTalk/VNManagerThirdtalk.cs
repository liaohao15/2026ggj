using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VNManagerThirdtalk : MonoBehaviour
{
    // 对齐原有UI命名
    public TextMeshProUGUI SpeakerName;
    public TextMeshProUGUI SpeekContent;
    public TypewriterEffectThirdtalk typewriterEffect;
    public Image AvatarImage;
    public AudioSource VocalAudio;

    // 按钮名改为thirdTofour
    public Button thirdTofour;
    // 目标场景索引（可根据需求修改，保持原有逻辑）
    public int targetSceneIndex = 8;

    // 自动读取ConstantsThirdtalk里的2.xlsx，无需改
    private string storyPath = ConstantsThirdtalk.STORY_PATH;
    private string defaultStoryFileName = ConstantsThirdtalk.DEFAULT_STORY_FILE_NAME;
    private List<ExcelReaderThirdtalk.ExcelDataThirdtalk> storyData;
    private int currentLine = 0;

    void Start()
    {
        // 初始化thirdTofour按钮
        if (thirdTofour != null)
        {
            thirdTofour.gameObject.SetActive(false);
            thirdTofour.onClick.AddListener(SwitchToTargetScene);
        }

        // 自动加载2.xlsx
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
        storyData = ExcelReaderThirdtalk.ReadExcel(path);
        if (storyData == null || storyData.Count == 0)
        {
            Debug.LogError(ConstantsThirdtalk.NO_DATA_FOUND);
        }
    }

    void DisplayNextLine()
    {
        if (storyData == null || currentLine >= storyData.Count)
        {
            Debug.Log(ConstantsThirdtalk.END_OF_STORY);
            if (thirdTofour != null)
            {
                thirdTofour.gameObject.SetActive(true);
            }
            return;
        }

        if (typewriterEffect.IsTyping())
        {
            typewriterEffect.CompleteLine();
        }
        else
        {
            if (thirdTofour != null)
            {
                thirdTofour.gameObject.SetActive(false);
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
        string imagePath = ConstantsThirdtalk.AVATAR_PATH + imageFileName;
        Sprite sprite = Resources.Load<Sprite>(imagePath);
        if (sprite != null)
        {
            AvatarImage.sprite = sprite;
            AvatarImage.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError(ConstantsThirdtalk.IMAGE_LOAD_FAILED + imagePath);
        }
    }

    void PlayVocalAudio(string audioFileName)
    {
        string audioPath = "ThirdTalk/audio/vocal/1";
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
            Debug.LogError(ConstantsThirdtalk.AUDIO_LOAD_FAILED + audioPath);
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

        if (currentLine >= storyData.Count && thirdTofour != null)
        {
            thirdTofour.gameObject.SetActive(true);
        }
    }

    // 切换到目标场景
    void SwitchToTargetScene()
    {
        try
        {
            SceneManager.LoadScene(targetSceneIndex);
            Debug.Log("成功切换到场景索引：" + targetSceneIndex);
        }
        catch (System.Exception e)
        {
            Debug.LogError("切换场景失败：" + e.Message);
        }
    }
}