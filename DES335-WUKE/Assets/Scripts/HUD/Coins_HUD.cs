using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coins_HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_Coins_Display;

    private float m_CurrentCoins_Value;
    private float m_TrueCoins_Value;
    private float m_Step_Value = 5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void InitHUD()
    {
        UpdateData();

        m_CurrentCoins_Value = m_TrueCoins_Value;

        //update text
        float temp = 1000000f;
        temp += m_CurrentCoins_Value;
        m_Coins_Display.text = temp.ToString().Remove(0, 1);
    }

    private void Update()
    {
        UpdateCoinsDisplay();
    }

    public void UpdateData()
    {
        m_TrueCoins_Value = GameManager.Instance.PlayerInstance.GetHoldingCoins();
    }

    private void UpdateCoinsDisplay()
    {
        UpdateData();

        if (m_CurrentCoins_Value == m_TrueCoins_Value)
        {
            return;
        }
        else if (m_CurrentCoins_Value < m_TrueCoins_Value)
        {
            float temp = 1000000f;

            m_CurrentCoins_Value += m_Step_Value;

            //check for over adding
            if (m_CurrentCoins_Value > m_TrueCoins_Value)
            {
                m_CurrentCoins_Value = m_TrueCoins_Value;
            }

            //update text
            temp += m_CurrentCoins_Value;

            m_Coins_Display.text = temp.ToString().Remove(0, 1);
        }
        else if (m_CurrentCoins_Value > m_TrueCoins_Value)
        {
            float temp = 1000000f;

            m_CurrentCoins_Value -= m_Step_Value;

            //check for over adding
            if (m_CurrentCoins_Value < m_TrueCoins_Value)
            {
                m_CurrentCoins_Value = m_TrueCoins_Value;
            }

            //update text
            temp += m_CurrentCoins_Value;

            m_Coins_Display.text = temp.ToString().Remove(0, 1);
        }
    }
}
