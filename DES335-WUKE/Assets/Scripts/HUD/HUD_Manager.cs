using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Manager : MonoBehaviour
{
    public static HUD_Manager Instance;

    [SerializeField] Stat_HUD m_Stat_HUD;
    [SerializeField] Weapon_HUD m_Weapon_HUD;
    [SerializeField] Coins_HUD m_Coin_HUD;
    [SerializeField] Trinket_HUD m_Trinket_HUD;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void InitHUD()
    {
        m_Coin_HUD.InitHUD();
        m_Stat_HUD.UpdateStatsDisplay();
        m_Weapon_HUD.UpdateWeaponDisplay();
    }

    public void ToggleWeaponHUD()
    {
        m_Weapon_HUD.ToggleWeaponAlpha();
    }

    public void UpdateHUD()
    {
        m_Stat_HUD.UpdateStatsDisplay();
        m_Weapon_HUD.UpdateWeaponDisplay();
    }

    public Trinket_HUD GetTrinketHUD()
    {
        return m_Trinket_HUD;
    }
}
