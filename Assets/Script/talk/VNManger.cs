using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 视觉小说场景控制器（导出后可稳定运行）
/// 功能：点击鼠标换图 → 最后一张显示按钮 → 点击按钮跳转场景
/// </summary>
public class VNSceneController : MonoBehaviour
{
    [Header("=== 必配组件（拖入对应UI） ===")]
    [Tooltip("背景图片的Image组件（必须是UI Image）")]
    public Image backgroundImage;
    [Tooltip("跳转场景的按钮组件")]
    public Button nextSceneButton;

    [Header("=== 场景配置 ===")]
    [Tooltip("要跳转的目标场景索引（看Build Settings里的顺序）")]
    public int targetSceneIndex = 3;
    [Tooltip("场景前缀（one/two/three/four，对应Resources里的文件夹）")]
    public string sceneFolderPrefix = "one";

    [Header("=== 图片配置（按显示顺序填） ===")]
    [Tooltip("图片名称列表（不用后缀，比如1、2、3）")]
    public List<string> imageNameList = new List<string>() { "1", "2", "3", "4" };

    // 内部变量（无需手动改）
    private int _currentImageIndex = 0; // 当前显示的图片索引
    private List<string> _fullImagePaths = new List<string>(); // 拼接后的完整图片路径

    void Start()
    {
        // 1. 初始化检查（防止漏配组件）
        if (backgroundImage == null)
        {
            Debug.LogError("请拖入背景Image组件！");
            return;
        }
        if (nextSceneButton == null)
        {
            Debug.LogError("请拖入跳转按钮组件！");
            return;
        }

        // 2. 拼接图片完整路径（适配Resources加载）
        foreach (string imgName in imageNameList)
        {
            string fullPath = $"{sceneFolderPrefix}/Image/Background/{imgName}";
            _fullImagePaths.Add(fullPath);
        }

        // 3. 初始隐藏跳转按钮
        nextSceneButton.gameObject.SetActive(false);
        // 绑定按钮点击事件
        nextSceneButton.onClick.AddListener(OnJumpButtonClick);

        // 4. 加载第一张图片
        LoadImage(_currentImageIndex);
    }

    void Update()
    {
        // 鼠标左键点击 → 切换图片（导出后依然有效）
        if (Input.GetMouseButtonDown(0))
        {
            // 已经到最后一张 → 显示按钮，不再切换
            if (_currentImageIndex >= _fullImagePaths.Count - 1)
            {
                nextSceneButton.gameObject.SetActive(true);
                return;
            }

            // 切换到下一张
            _currentImageIndex++;
            LoadImage(_currentImageIndex);
        }
    }

    /// <summary>
    /// 加载指定索引的图片（核心加载逻辑，导出后稳定）
    /// </summary>
    /// <param name="index">图片索引</param>
    private void LoadImage(int index)
    {
        // 防越界保护
        if (index < 0 || index >= _fullImagePaths.Count)
        {
            Debug.LogError($"图片索引错误：{index}，超出范围");
            return;
        }

        // Resources加载图片（Unity导出后最稳定的方式）
        Sprite targetSprite = Resources.Load<Sprite>(_fullImagePaths[index]);
        if (targetSprite != null)
        {
            backgroundImage.sprite = targetSprite;
            backgroundImage.preserveAspect = true; // 保持图片比例
            backgroundImage.gameObject.SetActive(true);
            Debug.Log($"成功加载图片：{_fullImagePaths[index]}");
        }
        else
        {
            Debug.LogError($"图片加载失败：{_fullImagePaths[index]}，请检查Resources路径");
            backgroundImage.color = Color.black; // 加载失败显示黑色
        }
    }

    /// <summary>
    /// 按钮点击 → 跳转场景
    /// </summary>
    private void OnJumpButtonClick()
    {
        // 检查场景索引是否有效
        if (targetSceneIndex < 0 || targetSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError($"场景索引错误：{targetSceneIndex}，请检查Build Settings");
            return;
        }

        // 跳转场景（导出后稳定执行）
        SceneManager.LoadScene(targetSceneIndex);
        Debug.Log($"跳转到场景：{targetSceneIndex}");
    }

    // ========== 导出前验证工具（可选） ==========
    [ContextMenu("验证所有图片路径")]
    private void ValidateImagePaths()
    {
        Debug.Log("开始验证图片路径...");
        foreach (string path in _fullImagePaths)
        {
            if (Resources.Load<Sprite>(path) != null)
            {
                Debug.Log($"验证通过：{path}");
            }
            else
            {
                Debug.LogError($"验证失败：{path}（图片不存在）");
            }
        }
    }
}