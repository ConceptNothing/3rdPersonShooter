using Cinemachine;
using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameOverScreen gameOverScreen;
    [SerializeField]
    private int score=1;
    [SerializeField]
    public float minDistanceFromPlayer = 1f;
    [SerializeField]
    public float maxDistanceFromPlayer = 2f;
    public float spawnRate = 25f;
    public float spawnRateIncreasePerScore = 0.1f;

    private CinemachineVirtualCamera[] cameras;

    EnemyController[] enemiesGameObjects;
    public GameObject enemyPrefab;
    private PlayerController[] players;

    // Start is called before the first frame update
    private void Start()
    {
        cameras = CinemachineVirtualCamera.FindObjectsOfType<CinemachineVirtualCamera>(true);
        StartCoroutine(SpawnEnemyCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        enemiesGameObjects = EnemyController.FindObjectsOfType<EnemyController>(true);
        players=PlayerController.FindObjectsOfType<PlayerController>(true);
        foreach(var player in players)
        {
            if (!player.gameObject.activeSelf)
            {
                Destroy(player);
                GameOver(true);
            }
        }
        if (enemiesGameObjects.Length > 0)
        {
            foreach (var enemy in enemiesGameObjects)
            {
                if (!enemy.gameObject.activeSelf)
                {
                    score++;
                    Destroy(enemy.gameObject);
                }
            }
        }
    }
    void SpawnEnemy()
    {
        Vector2 randomCirclePoint = Random.insideUnitCircle.normalized * Random.Range(minDistanceFromPlayer, maxDistanceFromPlayer);
        Vector3 enemySpawnPosition = new Vector3(transform.position.x + randomCirclePoint.x, 0f, transform.position.z + randomCirclePoint.y);
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(enemySpawnPosition.x, 50f, enemySpawnPosition.z), Vector3.down, out hit))
        {
            enemySpawnPosition.y = hit.point.y+15;
            Debug.Log("Enemy spawned at: " + enemySpawnPosition);
            GameObject newEnemy = Instantiate(enemyPrefab, enemySpawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("Failed to spawn enemy at: " + enemySpawnPosition);
        }
    }
    private IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length >= 50)
            {
                yield return new WaitForSeconds(10.0f);
                continue;
            }
                yield return new WaitForSeconds(spawnRate);

            SpawnEnemy();

            if (spawnRate > 0.1f)
            {
                spawnRate -= spawnRateIncreasePerScore * score;
            }
            else
            {
                spawnRate = 0.1f;
            }
        }
    }
    public void IncreaseScore(int amount)
    {
        score += amount;
    }

    public void GameOver(bool isWin)
    {
        foreach(var cam in cameras)
        {
            cam.gameObject.SetActive(false);
        }
        gameOverScreen.Setup(score);
    }
}
