using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponSwitching : MonoBehaviour
{
    public GameObject[] weapons;
    public int currentWeaponIndex = 0;
    public Rig[] armRigs;
    void Start()
    {
        weapons[currentWeaponIndex].SetActive(true);
        armRigs[currentWeaponIndex].weight += 1f;
    }

    void Update()
    {
        // Handle weapon weaponSwitch input & arm rig
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Deactivate the current weapon
            weapons[currentWeaponIndex].SetActive(false);
            armRigs[currentWeaponIndex].weight-=1f;
            currentWeaponIndex++;

            if (currentWeaponIndex >= weapons.Length)
            {
                currentWeaponIndex = 0;
            }

            // Activate the new weapon & arm rig
            weapons[currentWeaponIndex].SetActive(true);
            armRigs[currentWeaponIndex].weight += 1f;
        }
    }
}