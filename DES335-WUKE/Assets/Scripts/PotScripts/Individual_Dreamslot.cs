using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Individual_Dreamslot : MonoBehaviour
{
    //backing colour variables
    [SerializeField] Image m_SlotImage;
    [SerializeField] Color m_UnrolledColour;
    [SerializeField] Color m_NeutralColour;
    [SerializeField] Color m_GoodColour;
    [SerializeField] Color m_BadColour;

    //for dream selection
    [SerializeField] List<GameObject> m_DreamObj;
    [SerializeField] Vector2 m_NeutralDream_Index;
    [SerializeField] Vector2 m_GoodDream_Index;
    [SerializeField] Vector2 m_BadDream_Index;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SelectDream(string luckValue)
    {
        //toggle all settings off as a precaution
        for (int i = 0; i < m_DreamObj.Count; ++i)
        {
            m_DreamObj[i].SetActive(false);
        }

        int temp;
        List<int> activeDreams;

        switch (luckValue)
        {
            case "Neutral":
                Reroll_N:
                temp = Random.Range((int)m_NeutralDream_Index.x ,(int)m_NeutralDream_Index.y);
                m_SlotImage.color = m_NeutralColour;

                //rerolls the dream if it is already active
                activeDreams = GameManager.Instance.GetActiveDreams();
                for (int i = 0; i < activeDreams.Count; ++i)
                {
                    if (temp == activeDreams[i])
                    {
                        goto Reroll_N; //this will cause an infinite loop if all the dreams are active
                    }
                }
                break;
            case "Good":
                Reroll_G:
                temp = Random.Range((int)m_GoodDream_Index.x, (int)m_GoodDream_Index.y);
                m_SlotImage.color = m_GoodColour;

                //rerolls the dream if it is already active
                activeDreams = GameManager.Instance.GetActiveDreams();
                for (int i = 0; i < activeDreams.Count; ++i)
                {
                    if (temp == activeDreams[i])
                    {
                        goto Reroll_G;
                    }
                }
                break;
            case "Bad":
                Reroll_B:
                temp = Random.Range((int)m_BadDream_Index.x, (int)m_BadDream_Index.y);
                m_SlotImage.color = m_BadColour;

                //rerolls the dream if it is already active
                activeDreams = GameManager.Instance.GetActiveDreams();
                for (int i = 0; i < activeDreams.Count; ++i)
                {
                    if (temp == activeDreams[i])
                    {
                        goto Reroll_B;
                    }
                }
                break;
            default:
                temp = 0;
                Debug.Log("Select Dream Spoil");
                break;
        }

        //activates the selected dream
        m_DreamObj[temp].SetActive(true);
    }

    public void DisableDream()
    {
        for (int i = 0; i < m_DreamObj.Count; ++i)
        {
            m_DreamObj[i].SetActive(false);
        }
    }

    public void ResetSlot()
    {
        m_SlotImage.color = m_UnrolledColour;
    }
}
