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
    public float spawnRate = 5f;
    public float spawnRateIncreasePerScore = 0.1f;

    GameObject[] enemiesGameObjects;
    private string scoreKey = "Score";
    public GameObject enemyPrefab;


    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(SpawnEnemyCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        enemiesGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemiesGameObjects.Length >= 100)
        {
            StopCoroutine(SpawnEnemyCoroutine());
        }
    }
    void SpawnEnemy()
    {
        Vector3 spawnPosition;
        RaycastHit hit;

        do
        {
            Vector2 randomCirclePoint = Random.insideUnitCircle.normalized * Random.Range(minDistanceFromPlayer, maxDistanceFromPlayer);
            spawnPosition = new Vector3(transform.position.x + randomCirclePoint.x, 0f, transform.position.z + randomCirclePoint.y);
        }
        while (!Physics.Raycast(spawnPosition, Vector3.down, out hit, 100f, LayerMask.GetMask("Floor")));

        // Spawn enemy on top of floor
        spawnPosition.y = hit.point.y;
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
    private IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            // Wait for the specified amount of time before spawning an enemy
            yield return new WaitForSeconds(spawnRate);

            // Spawn a new enemy
            SpawnEnemy();

            // Update the spawn rate based on the current score
            spawnRate -= spawnRateIncreasePerScore * score;
        }
    }
    public void IncreaseScore(int amount)
    {
        score += amount;
    }

    public void GameOver(bool isWin)
    {
        gameOverScreen.Setup(score,isWin);
    }
}
