public class ConstantsFive
{
    // Excel文件路径（指向3.xlsx，非Resources加载，保留原路径）
    public static string STORY_PATH = "Assets/Five/Resources/story/";
    public static string DEFAULT_STORY_FILE_NAME = "3.xlsx"; // 核心改：1.xlsx→3.xlsx

    // 背景图片路径（关键改：删除Five/前缀，适配Resources加载规则）
    public static string BACKGROUND_PATH = "image/background/";
    // 音效文件路径（关键改：删除Five/前缀，适配Resources加载规则）
    public static string VOCAL_PATH = "audio/vocal/";

    // 错误提示常量
    public static string AUDIO_LOAD_FAILED = "Failed to load audio: ";
    public static string IMAGE_LOAD_FAILED = "Failed to load image: ";
    public static string NO_DATA_FOUND = "No data found";
    public static string END_OF_STORY = "End of story";
    // 保留无影响的常量
    public static float DEFAULT_WAITING_SECONDS = 0.05f;
}