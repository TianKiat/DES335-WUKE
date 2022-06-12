using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public float Damage { get; set; }
    public bool IsPlayerBullet { get; set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnHitResponse(collision.otherCollider.gameObject);
    }

    public virtual void OnHitResponse(GameObject other)
    {
        // Deal Damage here
        if (IsPlayerBullet)
        {
            // only deal damage to enemy types
        }
        else
        {
            // only deal damage to player types
        }
    }
}
