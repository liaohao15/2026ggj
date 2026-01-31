using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VNManager : MonoBehaviour
{
    // 核心引用
    public AudioSource vocalAudio;
    public Image BackgroundImage;
    public Button switchToThirdSceneBtn;
    public int targetSceneIndex = 3;

    // 音效文件名（匹配你的1.mp3）
    public string sceneVocalFileName = "1";

    // Excel路径
    private string storyPath = Constants.STORY_PATH;
    private string defaultStoryFileName = Constants.DEFAULT_STORY_FILE_NAME;
    private List<ExcelReader.ExcelData> storyData;
    private int currentLine = 0;

    void Start()
    {
        // 初始化按钮
        if (switchToThirdSceneBtn != null)
        {
            switchToThirdSceneBtn.gameObject.SetActive(false);
            switchToThirdSceneBtn.onClick.AddListener(SwitchToThirdScene);
        }

        // 加载Excel
        LoadStoryFromFile(storyPath + defaultStoryFileName);
        if (storyData != null && storyData.Count > 0)
        {
            PlaySceneVocal();
            DisplayThisLine();
        }
        else
        {
            Debug.LogError("未读取到Excel数据，无法加载背景");
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
        storyData = ExcelReader.ReadExcel(path);
        if (storyData == null || storyData.Count == 0)
        {
            Debug.LogError(Constants.NO_DATA_FOUND);
        }
    }

    void DisplayNextLine()
    {
        if (storyData == null || currentLine >= storyData.Count)
        {
            Debug.Log(Constants.END_OF_STORY);
            if (switchToThirdSceneBtn != null)
            {
                switchToThirdSceneBtn.gameObject.SetActive(true);
            }
            return;
        }
        DisplayThisLine();
    }

    void DisplayThisLine()
    {
        var data = storyData[currentLine];

        // 加载背景（适配Excel中的rbg1.jpg/rbg2.jpg）
        if (!string.IsNullOrEmpty(data.backgroundImageName))
        {
            UpdateBackgroundImage(data.backgroundImageName);
        }

        currentLine++;
    }

    void UpdateBackgroundImage(string imageFileName)
    {
        // 拼接路径：image/background/rbg1（去掉后缀）
        string imagePath = Constants.BACKGROUND_PATH + imageFileName.Replace(".jpg", "").Replace(".png", "");
        Sprite sprite = Resources.Load<Sprite>(imagePath);

        if (sprite != null)
        {
            BackgroundImage.sprite = sprite;
            BackgroundImage.gameObject.SetActive(true);
            Debug.Log("成功加载背景图：" + imageFileName);
        }
        else
        {
            Debug.LogError(Constants.IMAGE_LOAD_FAILED + imagePath);
            // 容错：加载失败时显示默认背景（可选）
            BackgroundImage.color = Color.black;
        }
    }

    void PlaySceneVocal()
    {
        string audioPath = Constants.VOCAL_PATH + sceneVocalFileName;
        AudioClip audioClip = Resources.Load<AudioClip>(audioPath);

        if (audioClip != null)
        {
            vocalAudio.Stop();
            vocalAudio.clip = audioClip;
            vocalAudio.Play();
            Debug.Log("成功播放音效：" + sceneVocalFileName + ".mp3");
        }
        else
        {
            Debug.LogError(Constants.AUDIO_LOAD_FAILED + audioPath);
        }
    }

    void SwitchToThirdScene()
    {
        try
        {
            SceneManager.LoadScene(targetSceneIndex);
            Debug.Log("成功切换到第三场景（索引3）");
        }
        catch (System.Exception e)
        {
            Debug.LogError("切换场景失败：" + e.Message);
        }
    }
}