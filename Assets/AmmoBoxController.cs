using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class AmmoBoxController : MonoBehaviour
{
    [SerializeField]
    WeaponController weaponController;
    private void Start()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collider that entered the zone is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            weaponController.AddAmmo(Random.Range(6, 30));
        }
    }
}
