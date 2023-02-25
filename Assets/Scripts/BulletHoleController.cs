using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHoleController : MonoBehaviour
{
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    float timeToDestroy=5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyObjectAfterDelay(bulletPrefab, timeToDestroy));
    }

    IEnumerator DestroyObjectAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }
}
