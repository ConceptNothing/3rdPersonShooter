using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDied : MonoBehaviour
{
    public delegate void EnemyDiedEventHandler(GameObject enemyObject);

    public static event EnemyDiedEventHandler EnemyDiedEvent;

    public static void NotifyEnemyDied(GameObject enemyObject)
    {
        if (EnemyDiedEvent != null)
        {
            EnemyDiedEvent(enemyObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
