using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Individual_Candyslot : MonoBehaviour
{
    //backing colour variables
    [SerializeField] Levelpot m_Levelpot;
    [SerializeField] Image m_SlotImage;
    [SerializeField] Color m_NeutralColour;
    [SerializeField] Color m_DisplayColour;

    //for Candy selection
    [SerializeField] List<GameObject> m_CandyObj;


    //for stat modification when player clicks on a candy
    private bool m_IsBasic;
    private string m_StatName_1;
    private string m_StatName_2;

    private void Start()
    {
        Resetslot();
    }

    public void RollCandySlot()
    {
        //this int will represent which index from the list will be the candy
        int temp = Random.Range(0, m_Levelpot.m_PossibleSelection.Count);

        //this is the candy index derived from the random selection
        int candyIndex = m_Levelpot.m_PossibleSelection[temp];

        //set image according to candy index
        DisplayCandyImage(candyIndex);

        SetButtonValues(candyIndex);
    }

    public void SelectCandy()
    {
        if (m_IsBasic == true)
        {
            GameManager.Instance.PlayerInstance.ModifyCurrentStat(m_StatName_1, 1);
        }
        else
        {
            GameManager.Instance.PlayerInstance.ModifyCurrentStat(m_StatName_1, 2);
            GameManager.Instance.PlayerInstance.ModifyCurrentStat(m_StatName_2, -1);
        }

        Debug.Log("Candy Selected");
        m_Levelpot.ResetLevelpot();
    }

    public void Resetslot()
    {
        m_SlotImage.color = m_NeutralColour;

        for (int i = 0; i < m_CandyObj.Count; ++i)
        {
            m_CandyObj[i].SetActive(false);
        }
    }

    private void DisplayCandyImage(int candyIndex)
    {
        m_SlotImage.color = m_DisplayColour;

        //turn off all images
        for (int i = 0; i < m_CandyObj.Count; ++i)
        {
            m_CandyObj[i].SetActive(false);
        }

        m_CandyObj[candyIndex].SetActive(true);
    }

    private void SetButtonValues(int candyIndex)
    {
        switch (candyIndex)
        {
            //basic candy
            //stb + 1
            case 0:
                m_IsBasic = true;
                m_StatName_1 = "stb";
                break;
            //lcd + 1
            case 1:
                m_IsBasic = true;
                m_StatName_1 = "lcd";
                break;
            //cog + 1
            case 2:
                m_IsBasic = true;
                m_StatName_1 = "cog";
                break;
            //opt + 1
            case 3:
                m_IsBasic = true;
                m_StatName_1 = "opt";
                break;
            
            //Stability bane
            //lcd +2 stb -1
            case 4:
                m_IsBasic = false;
                m_StatName_1 = "lcd";
                m_StatName_2 = "stb";
                break;
            //cog +2 stb -1
            case 5:
                m_IsBasic = false;
                m_StatName_1 = "cog";
                m_StatName_2 = "stb";
                break;
            //opt +2 stb -1
            case 6:
                m_IsBasic = false;
                m_StatName_1 = "opt";
                m_StatName_2 = "stb";
                break;

            //Lucidity bane
            //stb +2 lcd -1
            case 7:
                m_IsBasic = false;
                m_StatName_1 = "stb";
                m_StatName_2 = "lcd";
                break;
            //cog +2 lcd -1
            case 8:
                m_IsBasic = false;
                m_StatName_1 = "cog";
                m_StatName_2 = "lcd";
                break;
            //opt +2 lcd -1
            case 9:
                m_IsBasic = false;
                m_StatName_1 = "opt";
                m_StatName_2 = "lcd";
                break;

            //Cognition bane
            //stb +2 cog -1
            case 10:
                m_IsBasic = false;
                m_StatName_1 = "stb";
                m_StatName_2 = "cog";
                break;
            //lcd +2 cog -1
            case 11:
                m_IsBasic = false;
                m_StatName_1 = "lcd";
                m_StatName_2 = "cog";
                break;
            //opt +2 cog -1
            case 12:
                m_IsBasic = false;
                m_StatName_1 = "opt";
                m_StatName_2 = "cog";
                break;

            //Optimism bane
            //stb +2 opt -1
            case 13:
                m_IsBasic = false;
                m_StatName_1 = "stb";
                m_StatName_2 = "opt";
                break;
            //lcd +2 opt -1
            case 14:
                m_IsBasic = false;
                m_StatName_1 = "lcd";
                m_StatName_2 = "opt";
                break;
            //cog +2 opt -1
            case 15:
                m_IsBasic = false;
                m_StatName_1 = "cog";
                m_StatName_2 = "opt";
                break;
        }

    }
}
