using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxController : MonoBehaviour
{
    public GameObject healthBox;
    public GameObject ammoBox;

    void Start()
    {
        float randomNumber = Random.Range(0f, 1f);

        // If the random number is less than 0.5, enable the health box
        if (randomNumber < 0.5f)
        {
            healthBox.SetActive(true);
            ammoBox.SetActive(false);
        }
        // Otherwise, enable the ammo box
        else
        {
            healthBox.SetActive(false);
            ammoBox.SetActive(true);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
