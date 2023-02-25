using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoAmountUI : MonoBehaviour
{
    public Text ammoAmountText;
    public Weapon Weapon;
    // Update is called once per frame
    void Update()
    {
        ammoAmountText.text = Weapon.WeaponCurrentAmmo + "/" + Weapon.WeaponMaxAmmoAmount;
    }
}
