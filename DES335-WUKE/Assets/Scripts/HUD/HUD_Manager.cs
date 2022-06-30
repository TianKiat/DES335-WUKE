using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Manager : MonoBehaviour
{
    [SerializeField] Stat_HUD m_Stat_HUD;
    //[SerializeField] Trinket_HUD m_Trinket_HUD;
    [SerializeField] Coins_HUD m_Coin_HUD;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitHUD()
    {
        m_Coin_HUD.InitHUD();
        m_Stat_HUD.UpdateStatsDisplay();
    }

    public void UpdateHUD()
    {
        m_Stat_HUD.UpdateStatsDisplay();
    }
}
