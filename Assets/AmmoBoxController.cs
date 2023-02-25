using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AmmoBoxController : MonoBehaviour
{
    public Weapon pistol;
    public Weapon rifle;
    private int ammoAmount;
    private void Start()
    {
        ammoAmount = Random.Range(10,100);
    }
    private void OnCollisionEnter(Collision collision)
    {
        float randomNumber = Random.Range(0f, 1f);
        // Check if the collider that entered the zone is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // If the random number is less than 0.5, pistol ammo
            if (randomNumber < 0.5f)
            {
                pistol.WeaponCurrentAmmo += ammoAmount;
            }
            // Otherwise, enable the ammo box
            else
            {

            }
        }
    }
}
