using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float maxHealth=100f;
    [SerializeField]
    private GameObject LivingObject;
    private float currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth= maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth > maxHealth)
        {
            currentHealth= maxHealth;
        }
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.tag.ToString()+"I damaged with: "+damage+" units");
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Heal(float healAmount)
    {
        currentHealth += healAmount;    
    }
    private void Die()
    {

        if (gameObject.tag=="Enemy")
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
