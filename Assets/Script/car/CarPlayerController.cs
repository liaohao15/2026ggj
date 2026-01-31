using UnityEngine;

public class CarPlayerController : MonoBehaviour
{
    [Header("移动参数")]
    public float sideSpeed = 6f;
    public float roadLeftLimit = -3f; // 扩大左右移动范围
    public float roadRightLimit = 3f;
    private Vector3 initialPos; // 初始位置（重启复位用）

    void Start()
    {
        // 保存初始位置（加空值判断）
        if (gameObject != null)
        {
            initialPos = transform.position;
        }
    }

    void Update()
    {
        // 仅左右移动（A/D键）
        float sideInput = Input.GetAxis("Horizontal");
        Vector3 sideDir = new Vector3(sideInput, 0, 0) * sideSpeed * Time.deltaTime;
        transform.Translate(sideDir);

        // 限制X轴范围
        Vector3 currentPos = transform.position;
        currentPos.x = Mathf.Clamp(currentPos.x, roadLeftLimit, roadRightLimit);
        transform.position = currentPos;
    }

    // 外部调用：重置主角位置（加空值判断）
    public void ResetPosition()
    {
        if (gameObject != null)
        {
            transform.position = initialPos;
        }
    }
}