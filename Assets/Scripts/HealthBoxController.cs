using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoxController : MonoBehaviour
{
    private float healthAmount = 25f;
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collider that entered the zone is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("++HP");
            var playerHp=collision.gameObject.GetComponent<Health>();
            playerHp.Heal(healthAmount);
        }
    }
}
