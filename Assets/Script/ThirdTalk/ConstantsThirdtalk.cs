// 核心：删除路径中多余的ThirdTalk前缀，适配Resources加载规则
public class ConstantsThirdtalk
{
    // Excel文件路径（非Resources加载，保留原路径）
    public static string STORY_PATH = "Assets/ThirdTalk/Resources/story/";
    public static string DEFAULT_STORY_FILE_NAME = "2.xlsx";

    // 背景图片路径：Resources/image/background/（省略ThirdTalk和Resources）
    public static string BACKGROUND_PATH = "image/background/";
    // 音效路径：Resources/audio/vocal/（省略ThirdTalk和Resources）
    public static string VOCAL_PATH = "audio/vocal/";

    // 错误提示常量
    public static string AUDIO_LOAD_FAILED = "Failed to load audio: ";
    public static string IMAGE_LOAD_FAILED = "Failed to load image: ";
    public static string NO_DATA_FOUND = "No data found";
    public static string END_OF_STORY = "End of story";
}