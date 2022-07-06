using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfestedRatEnemy : GiantRatEnemy
{
    public GameObject[] wormsToSpawn;
    public GameObject worm;
    public override void Start()
    {
        base.Start();
        CurrentHealth = MaxHealth = BaseHealth = 40.0f;
    }
    protected override void Dying_Enter()
    {
        for (int i = 0; i < wormsToSpawn.Length; ++i)
        {
            Vector3 direction = Random.insideUnitCircle.normalized;

            wormsToSpawn[i].transform.SetParent(null);
            wormsToSpawn[i].SetActive(true);
            wormsToSpawn[i].GetComponent<Rigidbody2D>().AddForce(direction * Random.Range(5.0f, 10.0f), ForceMode2D.Impulse);
        }
        base.Dying_Enter();

    }

    void OnEnable()
    {
        for (int i = 0; i < wormsToSpawn.Length; ++i)
        {
            wormsToSpawn[i].SetActive(false);
        }
    }
}
