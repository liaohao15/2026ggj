using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VNManagerThirdtalk : MonoBehaviour
{
    // 核心引用
    public AudioSource VocalAudio;       // 音效播放组件
    public Image BackgroundImage;        // 背景显示组件
    public Button thirdTofour;           // 场景切换按钮
    public int targetSceneIndex = 8;     // 目标场景索引

    // 场景唯一音效文件名（填写：backmusic，无需.mp3后缀）
    public string sceneVocalFileName = "backmusic";

    // Excel数据存储
    private string storyPath = ConstantsThirdtalk.STORY_PATH;
    private string defaultStoryFileName = ConstantsThirdtalk.DEFAULT_STORY_FILE_NAME;
    private List<ExcelReaderThirdtalk.ExcelDataThirdtalk> storyData;
    private int currentLine = 0; // 当前背景行索引

    void Start()
    {
        // 初始化按钮：默认隐藏
        if (thirdTofour != null)
        {
            thirdTofour.gameObject.SetActive(false);
            thirdTofour.onClick.AddListener(SwitchToTargetScene);
        }

        // 加载Excel背景数据
        LoadStoryFromFile(storyPath + defaultStoryFileName);
        if (storyData != null && storyData.Count > 0)
        {
            PlaySceneVocal();    // 播放场景唯一音效
            DisplayThisLine();   // 显示第一张背景
        }
    }

    void Update()
    {
        // 鼠标左键切换下一张背景
        if (Input.GetMouseButtonDown(0))
        {
            DisplayNextLine();
        }
    }

    // 加载Excel中的背景数据
    void LoadStoryFromFile(string path)
    {
        storyData = ExcelReaderThirdtalk.ReadExcel(path);
        if (storyData == null || storyData.Count == 0)
        {
            Debug.LogError(ConstantsThirdtalk.NO_DATA_FOUND);
        }
    }

    // 切换下一张背景（无背景则显示按钮）
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
        DisplayThisLine();
    }

    // 显示当前行背景
    void DisplayThisLine()
    {
        var data = storyData[currentLine];

        // 加载并显示背景图片
        if (!string.IsNullOrEmpty(data.backgroundImageName))
        {
            UpdateBackgroundImage(data.backgroundImageName);
        }

        currentLine++; // 切换到下一行
    }

    // 加载背景图片并显示（Resources加载，自动识别jpg/png）
    void UpdateBackgroundImage(string imageFileName)
    {
        // 拼接路径：image/background/bg1（省略后缀）
        string imagePath = ConstantsThirdtalk.BACKGROUND_PATH + imageFileName.Replace(".jpg", "").Replace(".png", "");
        Sprite sprite = Resources.Load<Sprite>(imagePath);

        if (sprite != null)
        {
            BackgroundImage.sprite = sprite;
            BackgroundImage.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError(ConstantsThirdtalk.IMAGE_LOAD_FAILED + imagePath);
        }
    }

    // 播放场景唯一音效（Resources加载，无需后缀）
    void PlaySceneVocal()
    {
        // 拼接路径：audio/vocal/backmusic
        string audioPath = ConstantsThirdtalk.VOCAL_PATH + sceneVocalFileName;
        AudioClip audioClip = Resources.Load<AudioClip>(audioPath);

        if (audioClip != null)
        {
            VocalAudio.Stop();
            VocalAudio.clip = audioClip;
            VocalAudio.Play();
            Debug.Log("成功播放场景唯一音效：" + sceneVocalFileName);
        }
        else
        {
            Debug.LogError(ConstantsThirdtalk.AUDIO_LOAD_FAILED + audioPath);
        }
    }

    // 切换到目标场景（索引8）
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