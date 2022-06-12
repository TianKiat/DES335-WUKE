using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dreampot : MonoBehaviour
{
    [SerializeField] List<Individual_Dreamslot> m_IndividualSlots;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //feed in luck stat from player manager and roll chances for neutral/good/bad dreams
    public void RollLuckCheck(int Luck)
    {
        
    }

    private void RollSlots()
    {
        for (int i = 0; i < m_IndividualSlots.Count; ++i)
        {

        }
    }
}
