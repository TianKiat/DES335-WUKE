using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stat_HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_STB_Text;
    [SerializeField] TextMeshProUGUI m_LCD_Text;
    [SerializeField] TextMeshProUGUI m_COG_Text;
    [SerializeField] TextMeshProUGUI m_OPT_Text;

    // Start is called before the first frame update
    void Start()
    {
        UpdateStatsDisplay();
    }


    public void UpdateStatsDisplay()
    {
        m_STB_Text.text = ": " + GameManager.Instance.PlayerInstance.GetStat("stb").ToString();
        m_LCD_Text.text = ": " + GameManager.Instance.PlayerInstance.GetStat("lcd").ToString();
        m_COG_Text.text = ": " + GameManager.Instance.PlayerInstance.GetStat("cog").ToString();
        m_OPT_Text.text = ": " + GameManager.Instance.PlayerInstance.GetStat("opt").ToString();
    }
}
