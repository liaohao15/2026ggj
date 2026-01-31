using UnityEngine;

public class CarObstacleMovement : MonoBehaviour
{
    [HideInInspector] public float speed;
    [HideInInspector] public float destroyZ;

    void Update()
    {
        // 移动前检查自身是否有效
        if (gameObject == null) return;

        // 向南（Z轴负方向）移动
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        // 超出范围销毁（加空值判断）
        if (transform.position.z < destroyZ && gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    // 防止销毁时的引用问题
    void OnDestroy()
    {
        speed = 0;
        destroyZ = 0;
    }
}