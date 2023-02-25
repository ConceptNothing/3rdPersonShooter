using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletDecal;
    [SerializeField]
    private float timeToDestroy = 3f;

    [HideInInspector]
    public float BulletDamage { get; set; } = 10f;
    [HideInInspector]
    public float BulletSpeed { get; set; } = 10f;
    public Vector3 Target { get; set; }
    public bool Hit { get; set; }
     void OnEnable()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * BulletSpeed;
        Destroy(gameObject, timeToDestroy);
    }

     void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Target, BulletSpeed * Time.deltaTime);

        //.01f is small threshhold to represent near target distance
        //if threshhold is 0 it might never reach the target exactly
        if (Vector3.Distance(transform.position, Target) < 0.01f)
        {
            Hit = true;
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contactPoint = collision.GetContact(0);
        GameObject.Instantiate(bulletDecal, contactPoint.point+contactPoint.normal*.0001f,Quaternion.LookRotation(contactPoint.normal));
        Debug.Log("Bullet collided with " + collision.gameObject.name);

        //every bullet collision deals damage
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            var health = collision.gameObject.GetComponent<Health>();
            health.TakeDamage(BulletDamage);
        }
        Destroy(gameObject);
    }
    void OnTriggerEnter(Collider other)
    {
        // Get the closest point on the other collider's bounds to the trigger collider's position
        Vector3 contactPoint = other.ClosestPointOnBounds(transform.position);

        //Adjusted so bullet decal is properly rotated and positioned when colliding with floor or other non rigid bodies
        contactPoint.y = 1.04f;
        Quaternion rotation = Quaternion.Euler(100.0f, 0.0f, 0.0f);
        // Instantiate a bullet decal at the contact point
        GameObject.Instantiate(bulletDecal, contactPoint + other.transform.forward * .0001f, rotation);

        Debug.Log("Bullet collided with " + other.gameObject.name);

        Destroy(gameObject);
    }
}
