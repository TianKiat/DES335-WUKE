using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_HUD : MonoBehaviour
{
    private bool m_ActiveWeapon = false;

    [SerializeField] GameObject m_WeaponAlpha_1;
    [SerializeField] GameObject m_WeaponAlpha_2;

    [SerializeField] List<GameObject> m_Slot_1_Obj;
    [SerializeField] List<GameObject> m_Slot_2_Obj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleWeaponAlpha()
    {
        m_ActiveWeapon = !m_ActiveWeapon;

        m_WeaponAlpha_1.SetActive(m_ActiveWeapon);
        m_WeaponAlpha_2.SetActive(!m_ActiveWeapon);
    }

    public void UpdateWeaponDisplay()
    {
        Vector2 temp = GameManager.Instance.CheckWeapons();
        
        for (int i = 0; i < m_Slot_1_Obj.Count; ++i)
        {
            m_Slot_1_Obj[i].SetActive(false);
            m_Slot_2_Obj[i].SetActive(false);
        }

        if (temp.x == 0)
        {
            m_Slot_1_Obj[0].SetActive(true);
        }
        else if (temp.x == 1)
        {
            m_Slot_1_Obj[1].SetActive(true);
        }
        else if (temp.x == 2)
        {
            m_Slot_1_Obj[2].SetActive(true);
        }

        if (temp.y == 0)
        {
            m_Slot_2_Obj[0].SetActive(true);
        }
        else if (temp.y == 1)
        {
            m_Slot_2_Obj[1].SetActive(true);
        }
        else if (temp.y == 2)
        {
            m_Slot_2_Obj[2].SetActive(true);
        }

        Debug.Log(temp);
    }
}
