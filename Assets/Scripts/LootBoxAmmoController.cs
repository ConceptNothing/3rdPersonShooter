using UnityEngine;
public class LootBoxAmmoController : MonoBehaviour
{
    WeaponController weaponController;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var weapon = collision.gameObject.GetComponent<WeaponController>();
            weaponController = weapon;
            weaponController.AddAmmo(Random.Range(20, 30));
            Destroy(gameObject);
        }
    }
}
