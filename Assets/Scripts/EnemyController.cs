using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed = 30;
    [SerializeField]
    private float sightRange=400f;
    [SerializeField]
    private float detectionRange=200f;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform barrelTransform;
    [SerializeField]
    private Transform bulletParent;
    [SerializeField]
    private float fireRate = 1f;

    private float nextFireTime;
    private float speed;
    private Collider[] hitColliders;
    private RaycastHit hit;
    private bool isVisible;
    private float bulletHitMissDistance=25f;
    private LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        speed = maxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isVisible)
        {
            hitColliders = Physics.OverlapSphere(transform.position, detectionRange);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    target = hitCollider.gameObject;
                    isVisible = true;
                }
            }
        }
        else
        {
            if (Physics.Raycast(transform.position, (target.transform.position - transform.position ), out hit, sightRange))
            {
                if (!hit.collider.CompareTag("Player"))
                {
                    isVisible = false;
                }
                else
                {
                    //Get the direction
                    var heading = target.transform.position - transform.position;
                    var distance = heading.magnitude;
                    var direction = heading / distance;

                    //move to the player
                    Vector3 move = new Vector3(direction.x * speed, direction.y*speed, direction.z * speed);
                    rb.velocity = move;
                    transform.forward = move;

                    //Shoot
                    Shoot(fireRate);
                }
            }
        }
    }
    private void Shoot(float fireRate)
    {
        if (Time.time >= nextFireTime)
        {
            RaycastHit hit;
            // layer mask of the "Player"
            layerMask.value = 7;
            GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            //if there has been an target (ADD LAYER MASK IF NEEDED AT THE END OF THE Physics.Raycast)
            if (Physics.Raycast(barrelTransform.position, barrelTransform.forward, out hit, Mathf.Infinity,layerMask))
            {
                if (hit.collider != null)
                {
                    bulletController.Target = hit.point;
                    bulletController.Hit = true;
                }
            }
            //if shot was made in the sky
            else
            {
                bulletController.Target = barrelTransform.position + barrelTransform.forward * bulletHitMissDistance;
                bulletController.Hit = false;
            }

            nextFireTime = Time.time + fireRate;
        }
       
    }

}
