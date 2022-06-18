using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBullet : BaseBullet
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnHitResponse(GameObject other)
    {
        // Deal Damage here
        if (IsPlayerBullet)
        {
            BaseEnemy baseEnemy = other.GetComponent<BaseEnemy>();
            // only deal damage to enemy types
            if (baseEnemy != null)
                baseEnemy.TakeDamage(Damage);

        }

        Damage *= 1.1f;
    }
}
