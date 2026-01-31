using UnityEngine;

public class CarObstacleSpawner : MonoBehaviour
{
    [Header("生成参数")]
    public GameObject obstacleCarPrefab; // 障碍物预制体（必须赋值）
    [Tooltip("生成间隔（增大为3秒）")]
    public float spawnInterval = 3f;
    [Tooltip("障碍物速度（调快为10）")]
    public float obstacleSpeed = 10f;
    public float spawnRangeX = 1.2f; // 左右生成范围
    public float destroyZ = -10f;    // 销毁位置
    [Tooltip("第二个生成点物体")]
    public Transform obstacleSpawnerTwo;

    private float spawnTimer;
    private bool isSpawnEnabled = true;
    private bool useFirstSpawner = true; // 标记当前生成点

    void Update()
    {
        // 双重校验：是否允许生成 + 预制体是否为空
        if (!isSpawnEnabled || obstacleCarPrefab == null) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnObstacleCar();
            spawnTimer = 0;
            useFirstSpawner = !useFirstSpawner; // 切换生成点
        }
    }

    void SpawnObstacleCar()
    {
        // 生成前必检：预制体为空则直接返回，避免报错
        if (obstacleCarPrefab == null)
        {
            Debug.LogError("障碍物预制体（obstacleCarPrefab）未赋值或已销毁！");
            return;
        }

        Vector3 spawnPos;
        // 选择生成点（加空值判断）
        if (useFirstSpawner)
        {
            spawnPos = transform.position;
        }
        else
        {
            if (obstacleSpawnerTwo == null)
            {
                Debug.LogWarning("第二个生成点未赋值，使用第一个生成点");
                spawnPos = transform.position;
            }
            else
            {
                spawnPos = obstacleSpawnerTwo.position;
            }
        }

        // X轴随机偏移，贴合道路
        spawnPos.x += Random.Range(-spawnRangeX, spawnRangeX);
        spawnPos.y = 0.5f; // 固定高度

        // 生成障碍物（加空值保护）
        GameObject obstacleCar = Instantiate(obstacleCarPrefab, spawnPos, Quaternion.Euler(0, 90, 0));
        if (obstacleCar != null)
        {
            CarObstacleMovement moveScript = obstacleCar.AddComponent<CarObstacleMovement>();
            moveScript.speed = obstacleSpeed;
            moveScript.destroyZ = destroyZ;
        }
    }

    // 外部调用：停止生成
    public void StopSpawn()
    {
        isSpawnEnabled = false;
        spawnTimer = 0; // 重置计时器，避免恢复后立即生成
    }

    // 外部调用：重启生成（加空值判断）
    public void RestartSpawn()
    {
        if (obstacleCarPrefab == null)
        {
            Debug.LogError("预制体为空，无法重启生成！");
            return;
        }
        isSpawnEnabled = true;
        spawnTimer = 0;
        useFirstSpawner = true;
    }
}