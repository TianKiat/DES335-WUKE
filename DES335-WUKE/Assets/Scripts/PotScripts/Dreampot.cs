using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dreampot : MonoBehaviour
{
    [SerializeField] List<Individual_Dreamslot> m_IndividualSlots;

    List<string> m_LuckRoll = new List<string>();
    private Animator m_Anim;

    // Start is called before the first frame update
    void Start()
    {
        m_Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CloseDreampot()
    {
        m_Anim.Play("CloseDreampot");

        for (int i = 0; i < m_IndividualSlots.Count; ++i)
        {
            m_IndividualSlots[i].DisableDream();
        }
    }

    public void OpenDreampot()
    {
        m_Anim.Play("OpenDreampot");
    }

    //feed in luck stat from player manager and roll chances for neutral/good/bad dreams
    public void StartRolling(string luck)
    {
        m_LuckRoll.Clear();

        switch (luck)
        {
            case "Neutral":
                m_LuckRoll.Add("Neutral");
                m_LuckRoll.Add("Neutral");
                m_LuckRoll.Add("Neutral");
                break;
            case "Good":
                m_LuckRoll.Add("Good");
                m_LuckRoll.Add("Neutral");
                m_LuckRoll.Add("Neutral");
                break;
            case "Bad":
                m_LuckRoll.Add("Neutral");
                m_LuckRoll.Add("Neutral");
                m_LuckRoll.Add("Bad");
                break;
            default:

                Debug.Log("Roll luck check Spoil");
                break;
        }

        RollSlots(m_LuckRoll);
    }

    private void RollSlots(List<string> luckList)
    {
        for (int i = 0; i < m_IndividualSlots.Count; ++i)
        {
            m_IndividualSlots[i].SelectDream(m_LuckRoll[i]);
        }
    }
}
