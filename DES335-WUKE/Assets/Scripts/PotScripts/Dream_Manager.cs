using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dream_Manager : MonoBehaviour
{
    //dream characteristics
    [SerializeField] string m_PartyTime_Text;
    [SerializeField] string m_GymRats_Text;
    [SerializeField] string m_ItsJapanese_Text;
    [SerializeField] string m_DC_Neutral_1;
    [SerializeField] string m_DC_Neutral_2;
    [SerializeField] string m_DC_Neutral_3;

    //wake up conditions
    [SerializeField] string m_BreakingTheSeal_Text;
    [SerializeField] string m_Indegestion_Text;
    [SerializeField] string m_ActionHero_Text;
    [SerializeField] string m_WC_Neutral_1;
    [SerializeField] string m_WC_Neutral_2;
    [SerializeField] string m_WC_Neutral_3;


    [SerializeField] TextMeshProUGUI m_DisplayText;
    private int m_StoryValue;
    [SerializeField] private List<int> m_DreamsList = new List<int>();
    private string m_Template_Part1 = " The thought of ";
    private string m_Template_Part2 = " made Kevin go to school ";
    private string m_Storylet_Part1;
    private string m_Storylet_Part2;
    private string m_Storylet_Part3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateStorylet()
    {
        CheckActiveDreams();

        m_Storylet_Part1 = "";
        m_Storylet_Part2 = "";
        m_Storylet_Part3 = "";

        GenerateWakeUpCondition();
        GenerateDreamCharacteristis();
        GenerateStoryValue();

        m_DisplayText.text = m_Storylet_Part1 + m_Storylet_Part2 + m_Storylet_Part3;
    }

    private void GenerateWakeUpCondition()
    {
        //cycles through active dreams
        for (int i = 0; i < m_DreamsList.Count; ++i)
        {
            //no randomisation because i didnt take into account all possible values.
            //this is a temp solution
            switch (m_DreamsList[i])
            {
                case 1:
                    m_Storylet_Part1 = m_BreakingTheSeal_Text;
                    break;
                case 2:
                    m_Storylet_Part1 = m_Indegestion_Text;
                    break;
                case 5:
                    m_Storylet_Part1 = m_ActionHero_Text;
                    break;
            }
        }

        if (m_Storylet_Part1 == "")
        {
            int temp = Random.Range(0, 3);

            switch (temp)
            {
                case 0:
                    m_Storylet_Part1 = m_WC_Neutral_1;
                    break;
                case 1:
                    m_Storylet_Part1 = m_WC_Neutral_2;
                    break;
                default:
                    m_Storylet_Part1 = m_WC_Neutral_3;
                    break;
            }
        }
    }

    private void GenerateDreamCharacteristis()
    {
        m_Storylet_Part2 = m_Template_Part1;
        bool isFirst = true;

        for (int i = 0; i < m_DreamsList.Count; ++i)
        {
            //no randomisation because i didnt take into account all possible values.
            //this is a temp solution
            switch (m_DreamsList[i])
            {
                case 0:
                    //check if this is the first text input. if yes make it the first text, else put a & and then add the text
                    if (isFirst == true)
                    {
                        m_Storylet_Part2 += m_PartyTime_Text;
                        isFirst = false;
                    }
                    else
                    {
                        m_Storylet_Part2 += " and " + m_PartyTime_Text;
                    }
                    break;
                case 7:
                    if (isFirst == true)
                    {
                        m_Storylet_Part2 += m_GymRats_Text;
                        isFirst = false;
                    }
                    else
                    {
                        m_Storylet_Part2 += " and " + m_GymRats_Text;
                    }
                    break;
                case 8:
                    if (isFirst == true)
                    {
                        m_Storylet_Part2 += m_ItsJapanese_Text;
                        isFirst = false;
                    }
                    else
                    {
                        m_Storylet_Part2 += " and " + m_ItsJapanese_Text;
                    }
                    break;
            }
        }

        //if no text has been inputed use a default
        if (isFirst == true)
        {
            int temp = Random.Range(0, 3);

            switch (temp)
            {
                case 0:
                    m_Storylet_Part2 = m_DC_Neutral_1;
                    break;
                case 1:
                    m_Storylet_Part2 = m_DC_Neutral_2;
                    break;
                default:
                    m_Storylet_Part2 = m_DC_Neutral_3;
                    break;
            }
        }

        //finish mid section
        m_Storylet_Part2 += m_Template_Part2;
    }

    private void GenerateStoryValue()
    {
        //reset to 0
        m_StoryValue = 0;

        //calculate value
        for (int i = 0; i < m_DreamsList.Count; ++i)
        { 
            switch (m_DreamsList[i])
            {
                case 0:
                    break;
                case 3:
                case 4:
                case 5:
                    m_StoryValue += 1;
                    break;
                case 2:
                case 6:
                case 7:
                case 8:
                    m_StoryValue += -1;
                    break;
                case 1:
                    m_StoryValue += -2;
                    break;

            }
        }

        //write text
        if (m_StoryValue == 0)
        {
            m_Storylet_Part3 = "as if nothing weird occured in his dreams.";
        }
        else if (m_StoryValue > 0)
        {
            m_Storylet_Part3 = "with a smile on his face.";
        }
        else
        {
            m_Storylet_Part3 = "rather disturbed by his imagination...";
        }
    }
    public void CheckActiveDreams()
    {
        m_DreamsList.Clear();
        m_DreamsList = GameManager.Instance.GetActiveDreams();
    }
}
