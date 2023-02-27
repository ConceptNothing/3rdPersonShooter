using UnityEngine;
public class LootBoxHpController : MonoBehaviour
{
    private float healthAmount = 25f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("++HP");
            var playerHp = collision.gameObject.GetComponent<Health>();
            playerHp.Heal(healthAmount);
            Destroy(gameObject);
        }
    }
}
