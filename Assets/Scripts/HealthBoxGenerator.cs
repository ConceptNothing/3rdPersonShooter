using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoxGenerator : MonoBehaviour
{
    public GameObject prefab;
    public int numObjects;
    public float minDistance;
    public float maxDistance;
    public float minHeight;
    public Transform floor;

    void Start()
    {
        // Get an array of all game objects in the scene
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();

        for (int i = 0; i < numObjects; i++)
        {
            GameObject randomObject = null;
            Vector3 randomObjectPosition = Vector3.zero;

            // Keep choosing a random game object until we find one that is above the minimum height
            while (randomObject == null || randomObjectPosition.y < minHeight)
            {
                // Choose a random game object from the array
                randomObject = objects[Random.Range(0, objects.Length)];

                // Get the position of the random object
                randomObjectPosition = randomObject.transform.position;
            }

            // Generate a random position around the random object
            Vector3 randomPosition = randomObjectPosition + Random.insideUnitSphere * Random.Range(minDistance, maxDistance);

            // Instantiate the new object at the random position
            Instantiate(prefab, randomPosition, Quaternion.identity);
        }
    }
}
