using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponSystem : MonoBehaviour
{
    [SerializeField]
    private BaseWeapon[] WeaponLoadOut;
    private int currentWeaponIndex = 0;

    private int[] currentWeaponLoadOut = { 0, 1 };

    public float WeaponsDamageModifier { get; set; } = 0.0f;
    public float ReloadSpeedModifier { get; set; } = 0.0f;

    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI reloadText;

    public void FireWeapon()
    {
        if (currentWeaponIndex >= 0 && currentWeaponIndex < currentWeaponLoadOut.Length)
            WeaponLoadOut[currentWeaponLoadOut[currentWeaponIndex]].FireWeapon(WeaponsDamageModifier);
    }

    public void ReloadWeapon()
    {
        if (currentWeaponIndex >= 0 && currentWeaponIndex < currentWeaponLoadOut.Length)
            WeaponLoadOut[currentWeaponLoadOut[currentWeaponIndex]].ReloadWeapon(ReloadSpeedModifier);
    }

    public void SwitchWeapon()
    {
        //// disable all weapons
        //foreach (BaseWeapon weapon in WeaponLoadOut)
        //    weapon.gameObject.SetActive(false);

        //// enable current weapon
        //WeaponLoadOut[currentWeaponLoadOut[currentWeaponIndex]].gameObject.SetActive(true);

        //Check if 2nd slot is empty
        if (WeaponLoadOut[currentWeaponLoadOut[1]] != null)
        {
            //Disable current weapon and enable next weapon
            WeaponLoadOut[currentWeaponLoadOut[currentWeaponIndex]].gameObject.SetActive(false);
            currentWeaponIndex = currentWeaponIndex == 0 ? 1 : 0;
            WeaponLoadOut[currentWeaponLoadOut[currentWeaponIndex]].gameObject.SetActive(true);
        }
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

    private void Update()
    {
        // update ammo text
        ammoText.text = "" + WeaponLoadOut[currentWeaponLoadOut[currentWeaponIndex]].CurrentMagazineCapacity
            + " / " + WeaponLoadOut[currentWeaponLoadOut[currentWeaponIndex]].MagazineCapacity;


        reloadText.gameObject.SetActive(IsReloading());

    }

    public void PickUp(BaseWeapon weapon, ref float playerCurrency)
    {
        //Check if weapon has a price and if holding enough currency
        if (weapon.GetIsShopWeapon() && playerCurrency > weapon.GetCoinCost())
        {
            //Check if 2nd slot is empty, if empty add to it
            if (WeaponLoadOut[currentWeaponLoadOut[1]] == null)
            {
                AddWeaponToInventory(weapon);
            }
            //Else replace with current slot item
            else
            {
                SwapWeaponFromGround(weapon);
            }
        }
        //Enemy drop
        else
        {
            //Check if 2nd slot is empty, if empty add to it
            if (WeaponLoadOut[currentWeaponLoadOut[1]] == null)
            {
                AddWeaponToInventory(weapon);
            }
            else
            {
                SwapWeaponFromGround(weapon);
            }
        }
    }

    void AddWeaponToInventory(BaseWeapon weapon)
    {
        weapon.isOnGround = false;
        WeaponLoadOut[currentWeaponLoadOut[1]] = weapon;
        ResetTransform(weapon);
        weapon.gameObject.SetActive(false);
        SwitchWeapon();
    }

    void SwapWeaponFromGround(BaseWeapon weapon)
    {
        //Detach from hand and drop on ground
        WeaponLoadOut[currentWeaponLoadOut[currentWeaponIndex]].gameObject.transform.parent = null;
        WeaponLoadOut[currentWeaponLoadOut[currentWeaponIndex]].isOnGround = true;
        WeaponLoadOut[currentWeaponLoadOut[currentWeaponIndex]].GetComponent<BoxCollider2D>().enabled = true;
        WeaponLoadOut[currentWeaponLoadOut[currentWeaponIndex]].transform.localRotation = Quaternion.identity;
        
        //Assign new item to hand
        weapon.isOnGround = false;
        weapon.GetComponent<BoxCollider2D>().enabled = false;
        WeaponLoadOut[currentWeaponLoadOut[currentWeaponIndex]] = weapon;
        ResetTransform(weapon);
    }

    void ResetTransform(BaseWeapon weapon)
    {
        weapon.gameObject.transform.parent = GetComponent<PlayerController>().GetWeaponPivot();
        weapon.gameObject.transform.localPosition = Vector3.zero;
        weapon.gameObject.transform.localRotation = Quaternion.identity;
        weapon.gameObject.transform.localScale = new Vector3(-0.5f, 0.5f, 1.0f);
    }
}
