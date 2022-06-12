using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Individual_Dreamslot : MonoBehaviour
{
    //backing colour variables
    [SerializeField] GameObject m_SlotObj;
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
        //toggle all settings off as a precaution
        for (int i = 0; i < m_DreamObj.Count; ++i)
        {
            m_DreamObj[i].SetActive(false);
        }
    }

    public void SelectDream(string luckValue)
    {
        int temp;

        switch (luckValue)
        {
            case "Neutral":
                temp = Random.Range((int)m_NeutralDream_Index.x ,(int)m_NeutralDream_Index.y);
                break;
            case "Good":
                temp = Random.Range((int)m_GoodDream_Index.x, (int)m_GoodDream_Index.y);
                break;
            case "Bad":
                temp = Random.Range((int)m_BadDream_Index.x, (int)m_BadDream_Index.y);
                break;
            default:
                temp = 0;
                Debug.Log("Select Dream Spoil");
                break;
        }

        //activates the selected dream
        m_DreamObj[temp].SetActive(true);
    }
}
