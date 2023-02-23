using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletDecal;
    [SerializeField]
    private float damage=25f;
    private float speed = 10f;
    private float timeToDestroy = 3f;

    public Vector3 Target { get; set; }
    public bool Hit { get; set; }
    private void OnEnable()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        Destroy(gameObject, timeToDestroy);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Target, speed * Time.deltaTime);

        //.01f is small threshhold to represent near target distance
        //if threshhold is 0 it might never reach the target exactly
        if (Vector3.Distance(transform.position, Target) < 0.01f)
        {
            Hit = true;
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contactPoint = collision.GetContact(0);
        GameObject.Instantiate(bulletDecal, contactPoint.point+contactPoint.normal*.0001f,Quaternion.LookRotation(contactPoint.normal));
        Debug.Log("Bullet collided with " + collision.gameObject.name);

        //every bullet collision deals damage
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            var health = collision.gameObject.GetComponent<Health>();
            health.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
