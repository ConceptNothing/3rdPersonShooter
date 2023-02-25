using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int WeaponCurrentAmmo;

    public int WeaponMaxAmmoAmount;
    public float WeaponBulletSpeed;
    public float WeaponShootingRate;
    public float WeaponDamage;

    private void Awake()
    {
        WeaponCurrentAmmo = WeaponMaxAmmoAmount;
    }
}
