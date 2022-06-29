using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Interact_Popup : MonoBehaviour
{
    [SerializeField] GameObject m_E_Popup;

    // Start is called before the first frame update
    void Start()
    {
        m_E_Popup.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            m_E_Popup.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            m_E_Popup.SetActive(false);
        }
    }
}
