using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class HealthOrb : MonoBehaviour
{
    float amount;

    public void SetAmount(float value)
    {
        amount = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Only player can pick it up
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().AddHealth(amount);
            Destroy(gameObject);
        }
    }
}
