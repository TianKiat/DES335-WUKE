using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [SerializeField]
    private BaseWeapon[] WeaponLoadOut;
    private int currentWeaponIndex = 0;

    private int[] currentWeaponLoadOut = { 0, 1};

    public void FireWeapon()
    {
        if (currentWeaponIndex >= 0 && currentWeaponIndex < currentWeaponLoadOut.Length)
            WeaponLoadOut[currentWeaponLoadOut[currentWeaponIndex]].FireWeapon();
    }

    public void ReloadWeapon()
    {
        if (currentWeaponIndex >= 0 && currentWeaponIndex < currentWeaponLoadOut.Length)
            WeaponLoadOut[currentWeaponLoadOut[currentWeaponIndex]].ReloadWeapon();
    }

    public void SwitchWeapon()
    {
        currentWeaponIndex = currentWeaponIndex == 0 ? 1 : 0;

        // disable all weapons
        foreach (BaseWeapon weapon in WeaponLoadOut)
            weapon.gameObject.SetActive(false);

        // enable current weapon
        WeaponLoadOut[currentWeaponLoadOut[currentWeaponIndex]].gameObject.SetActive(true);
    }

    // provide a weapon_id and the currently equipped weapon will be swapped to that weapon_id
    // weapon_id:
    //              0 -> Finger Pistol
    //              1 -> Poopy Finger Pistol
    //              2 -> Ghostly Finger Pistol
    public void ChangeWeaponLoadOut(int weapon_id)
    {
        currentWeaponLoadOut[currentWeaponIndex] = weapon_id;
    }

    public bool IsReloading()
    {
        return WeaponLoadOut[currentWeaponLoadOut[currentWeaponIndex]].GetIsReloading();
    }

    public BaseWeapon GetCurrentWeapon()
    {
        if (currentWeaponIndex >= 0 && currentWeaponIndex < currentWeaponLoadOut.Length)
            return WeaponLoadOut[currentWeaponLoadOut[currentWeaponIndex]];

        return null;
    }
}
