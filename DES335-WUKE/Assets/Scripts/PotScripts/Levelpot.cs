using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levelpot : MonoBehaviour
{
    [SerializeField] List<Individual_Candyslot> m_IndividualSlots;

    private Animator m_Anim;

    [SerializeField] List<int> m_BasicIndexes;
    [SerializeField] List<int> m_StabilityBaneIndexes;
    [SerializeField] List<int> m_LucidityBaneIndexes;
    [SerializeField] List<int> m_CognitionBaneIndexes;
    [SerializeField] List<int> m_OpptimismIndexes;
    public List<int> m_PossibleSelection = new List<int>();

    [SerializeField] GameObject m_Ballbutton;
    [SerializeField] GameObject m_Closebutton;
    [SerializeField] GameObject m_CostUI;

    // Start is called before the first frame update
    void Start()
    {
        m_Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CloseLevelpot()
    {
        m_Anim.Play("CloseLevelpot");
    }

    public void OpenLevelpot()
    {
        m_Anim.Play("OpenLevelpot");
    }

    public void StartRolling()
    {
        //update possible candies. if a stat is 1 that bane candy should not appear
        UpdatePossibleSelections();

        m_Anim.Play("RollLevelpot");

        m_Ballbutton.SetActive(false);
        m_Closebutton.SetActive(false);
        m_CostUI.SetActive(false);
    }

    public void ResetLevelpot()
    {
        for (int i = 0; i < m_IndividualSlots.Count; ++i)
        {
            m_IndividualSlots[i].Resetslot();
        }

        m_Ballbutton.SetActive(true);
        m_Closebutton.SetActive(true);
        m_CostUI.SetActive(true);
    }

    private void RollSlots()
    {
        for (int i = 0; i < m_IndividualSlots.Count; ++i)
        {
            m_IndividualSlots[i].RollCandySlot();
        }
    }

    private void UpdatePossibleSelections()
    {
        m_PossibleSelection.Clear();

        //add in basic candy indexes
        for (int i = 0; i < m_BasicIndexes.Count; ++i)
        {
            m_PossibleSelection.Add(m_BasicIndexes[i]);
        }

        //check if the individual stat is more than 1
        if (GameManager.Instance.PlayerInstance.GetStat("stb") > 1)
        {
            //when a stat is more than 1, that bane candy will be added to the pool
            for (int i = 0; i < m_StabilityBaneIndexes.Count; ++i)
            {
                m_PossibleSelection.Add(m_StabilityBaneIndexes[i]);
            }
        }
        if (GameManager.Instance.PlayerInstance.GetStat("lcd") > 1)
        {
            for (int i = 0; i < m_StabilityBaneIndexes.Count; ++i)
            {
                m_PossibleSelection.Add(m_LucidityBaneIndexes[i]);
            }
        }
        if (GameManager.Instance.PlayerInstance.GetStat("cog") > 1)
        {
            for (int i = 0; i < m_StabilityBaneIndexes.Count; ++i)
            {
                m_PossibleSelection.Add(m_CognitionBaneIndexes[i]);
            }
        }
        if (GameManager.Instance.PlayerInstance.GetStat("opt") > 1)
        {
            for (int i = 0; i < m_StabilityBaneIndexes.Count; ++i)
            {
                m_PossibleSelection.Add(m_OpptimismIndexes[i]);
            }
        }
    }
}
