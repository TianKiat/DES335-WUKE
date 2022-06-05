using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [SerializeField]
    private BaseWeapon[] WeaponLoadOut;
    private int currentWeaponIndex = 0;

    public void Start()
    {
    }

    public void FireWeapon()
    {
        if (currentWeaponIndex >= 0 && currentWeaponIndex < WeaponLoadOut.Length)
            WeaponLoadOut[currentWeaponIndex].FireWeapon();
    }

    public void ReloadWeapon()
    {
        if (currentWeaponIndex >= 0 && currentWeaponIndex < WeaponLoadOut.Length)
            WeaponLoadOut[currentWeaponIndex].ReloadWeapon();
    }

    public void SwitchWeapon()
    {
        if (++currentWeaponIndex > WeaponLoadOut.Length - 1)
            currentWeaponIndex = 0;

        // disable all weapons
        foreach (BaseWeapon weapon in WeaponLoadOut)
            weapon.gameObject.SetActive(false);

        // enable current weapon
        WeaponLoadOut[currentWeaponIndex].gameObject.SetActive(true);
    }

    public bool IsReloading()
    {
        return WeaponLoadOut[currentWeaponIndex].GetIsReloading();
    }

    public BaseWeapon GetCurrentWeapon()
    {
        if (currentWeaponIndex >= 0 && currentWeaponIndex < WeaponLoadOut.Length)
            return WeaponLoadOut[currentWeaponIndex];

        return null;
    }
}
