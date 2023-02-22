using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletDecal;

    private float speed = 50f;
    private float timeToDestroy = 3f;

    public Vector3 Target { get; set; }
    public bool Hit { get; set; }
    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Target, speed * Time.deltaTime);

        //.01f is small threshhold to represent near target distance
        //if threshhold is 0 it might never reach the target exactly
        if (!Hit && Vector3.Distance(transform.position, Target) < .01f)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contactPoint = collision.GetContact(0);
        GameObject.Instantiate(bulletDecal, contactPoint.point+contactPoint.normal*.0001f,Quaternion.LookRotation(contactPoint.normal));
        //Destroy the object after reaching target
        Destroy(gameObject);
    }
}
