using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxHpController : MonoBehaviour
{
    public HealthBoxController healthBox;
    void Awake()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
