public class ConstantsTalk
{
    // Excel文件路径（指向4.xlsx）
    public static string STORY_PATH = "Assets/Talk/Resources/story/";
    public static string DEFAULT_STORY_FILE_NAME = "4.xlsx"; // 保留4.xlsx

    // 新增：背景图片路径（删除Talk/前缀，适配Resources规则）
    public static string BACKGROUND_PATH = "image/background/";
    // 音效路径（删除Talk/前缀，适配Resources规则）
    public static string VOCAL_PATH = "audio/vocal/";

    // 保留错误提示
    public static string AUDIO_LOAD_FAILED = "Failed to load audio: ";
    public static string IMAGE_LOAD_FAILED = "Failed to load image: ";
    public static string NO_DATA_FOUND = "No data found";
    public static string END_OF_STORY = "End of story";
    // 保留无影响的常量
    public static float DEFAULT_WAITING_SECONDS = 0.05f;
}