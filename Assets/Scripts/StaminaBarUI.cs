using UnityEngine;
using UnityEngine.UI;

public class StaminaBarUI : MonoBehaviour
{
    public Image staminaBar;
    public PlayerController playerController;

    // Update is called once per frame
    void Update()
    {
        float currentStamina = playerController.GetStamina();
        float maxStamina = playerController.GetMaxStamina();
        staminaBar.fillAmount = currentStamina / maxStamina;
    }
}