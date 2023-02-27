using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int CurrentAmmo;
    public int MaxAmmoAmount;
    public int ClipSize;
    public float BulletSpeed;
    public float ShootingRate;
    public float Damage;
    [HideInInspector]
    public int CurrentClipAmmo;
    public float ReloadSpeed;

    private void Awake()
    {
        CurrentAmmo = MaxAmmoAmount;
        CurrentClipAmmo = ClipSize;
    }
}
