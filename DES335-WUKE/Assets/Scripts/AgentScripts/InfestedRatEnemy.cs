using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfestedRatEnemy : GiantRatEnemy
{
    public int wormsToSpawn = 5;
    public GameObject worm;
    public override void Start()
    {
        base.Start();
        CurrentHealth = MaxHealth = BaseHealth = 40.0f;
    }
    protected override void Dying_Enter()
    {
        for (int i = 0; i < wormsToSpawn; ++i)
        {
            Vector3 position = transform.position + new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-2.5f, 2.5f), 0.0f);

            Instantiate(worm, position, Quaternion.identity);
        }
        base.Dying_Enter();

    }
}
