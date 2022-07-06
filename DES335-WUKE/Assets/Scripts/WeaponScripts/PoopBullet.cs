using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopBullet : BaseBullet
{
    // Start is called before the first frame update


    private static float DOTTickRate = 0.5f;

    public override void OnHitResponse(GameObject other)
    {
        // Deal Damage here
        if (IsPlayerBullet)
        {
            BaseEnemy baseEnemy = other.GetComponent<BaseEnemy>();
            // only deal damage to enemy types
            if (baseEnemy != null)
            {
                baseEnemy.ApplyDOT(DOT(4, Damage, baseEnemy));
                Destroy(gameObject);
                AddKnockback(other);
            }

        }
    }

    IEnumerator DOT(int ticks, float damage, BaseEnemy enemy)
    {
        for (int i = 0; i < ticks; ++i)
        {
            enemy.TakeDamage(damage);
            yield return new WaitForSeconds(DOTTickRate);

            // spawn or play some vfx
        }
        yield return null;
    }
}
