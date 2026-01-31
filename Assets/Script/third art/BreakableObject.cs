using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [Header("破坏效果")]
    public float destroyDelay = 0.1f;     // 破坏延迟
    public BreakableObjectsManager manager; // 管理器引用

    private bool isBroken = false;        // 是否已破坏

    // 鼠标点击破坏（保留，适配手动测试）
    private void OnMouseDown()
    {
        BreakObject();
    }

    // 【仅注释/删除这部分】移除碰撞中的Input检测（避免生命周期冲突）
    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.collider.CompareTag("Player") && Input.GetKey(KeyCode.Mouse0))
    //     {
    //         BreakObject();
    //     }
    // }

    // 破坏物体（核心逻辑完全不变）
    public void BreakObject()
    {
        if (isBroken) return;

        isBroken = true;

        // 通知管理器
        if (manager != null)
        {
            manager.OnObjectBroken();
        }

        // 破坏效果（隐藏物体，逻辑不变）
        gameObject.SetActive(false);

        // 可选：延迟销毁（如需彻底删除物体，取消注释）
        // Destroy(gameObject, destroyDelay);
    }
}