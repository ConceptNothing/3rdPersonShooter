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
    [SerializeField]
    private float lootboxSpawnRate = 5;
    [SerializeField]
    public float enemySpawnRate = 25f;
    [SerializeField]
    public float spawnRateIncreasePerScore = 0.1f;
    [SerializeField]
    public GameObject lootboxPrefab;

    private CinemachineVirtualCamera[] cameras;

    EnemyController[] enemiesGameObjects;
    public GameObject enemyPrefab;
    private PlayerController[] players;

    // Start is called before the first frame update
    private void Start()
    {
        cameras = CinemachineVirtualCamera.FindObjectsOfType<CinemachineVirtualCamera>(true);
        StartCoroutine(SpawnEnemyCoroutine());
        StartCoroutine(SpawnLootBoxCoroutine());
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
    void SpawnObject(GameObject obj)
    {
        Vector2 randomCirclePoint = Random.insideUnitCircle.normalized * Random.Range(minDistanceFromPlayer, maxDistanceFromPlayer);
        Vector3 objectSpawnPosition = new Vector3(transform.position.x + randomCirclePoint.x, 0f, transform.position.z + randomCirclePoint.y);
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(objectSpawnPosition.x, 50f, objectSpawnPosition.z), Vector3.down, out hit))
        {
            objectSpawnPosition.y = hit.point.y+15;
            Debug.Log("Object spawned at: " + objectSpawnPosition);
            GameObject newObject = Instantiate(obj, objectSpawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("Failed to spawn object at: " + objectSpawnPosition);
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
                yield return new WaitForSeconds(enemySpawnRate);

            SpawnObject(enemyPrefab);

            if (enemySpawnRate > 0.1f)
            {
                enemySpawnRate -= spawnRateIncreasePerScore * score;
            }
            else
            {
                enemySpawnRate = 0.1f;
            }
        }
    }
    private IEnumerator SpawnLootBoxCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(lootboxSpawnRate);

            SpawnObject(lootboxPrefab);
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
