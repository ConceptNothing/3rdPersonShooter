using UnityEngine.UI;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    public Image healthBar;
    public Health objectHealth;

    // Update is called once per frame
    void Update()
    {
        float currentHealth = objectHealth.GetCurrentHealth();
        float maxHealth = objectHealth.GetMaxHealth();
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
