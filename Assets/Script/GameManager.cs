using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("场景切换设置")]
    public float switchTime = 30f; // 30秒切换场景

    [Header("组件引用")]
    public CarObstacleSpawner obstacleSpawner;
    public CarPlayerController playerCar;

    private float gameTimer;
    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver) return;

        gameTimer += Time.deltaTime;
        // 30秒到了直接切换到 third act 场景
        if (gameTimer >= switchTime)
        {
            SceneManager.LoadScene(4);
        }
    }

    // 碰撞到障碍物触发
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ObstacleCar") && !isGameOver)
        {
            isGameOver = true;
            obstacleSpawner.StopSpawn(); // 停止生成障碍物
            Invoke("RestartGame", 1f); // 1秒后重启游戏
        }
    }

    // 重启游戏
    void RestartGame()
    {
        // 销毁所有障碍物
        GameObject[] obstacleCars = GameObject.FindGameObjectsWithTag("ObstacleCar");
        foreach (var car in obstacleCars)
        {
            Destroy(car);
        }

        // 重置参数
        gameTimer = 0;
        isGameOver = false;
        obstacleSpawner.RestartSpawn();
        playerCar.ResetPosition();
    }
}
