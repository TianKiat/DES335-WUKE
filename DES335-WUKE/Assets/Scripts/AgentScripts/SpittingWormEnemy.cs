using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpittingWormEnemy : BaseEnemy
{
    public float AttackDelay = 0.2f;

    public BaseWeapon weapon;

    public float range = 5.0f;

    private Transform playerTransform;
    private float radiusSqr;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        GetComponent<Rigidbody2D>().isKinematic = true;

        radiusSqr = (range * range);

        playerTransform = GameManager.Instance.PlayerInstance.transform;

        CurrentHealth = MaxHealth = BaseHealth = 10.0f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (GameManager.Instance.GetPauseState())
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector3 dir = playerTransform.position - transform.position;
        if (dir.sqrMagnitude <= radiusSqr)
        {
            weapon.transform.right = dir;
            BaseBullet bullet = weapon.FireWeapon(DamageModifier);
            if (bullet)
                bullet.IsPlayerBullet = false;
        }
    }

    void Dying_Enter()
    {
        fsm.ChangeState(States.Dead);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (CurrentHealth <= 0.0f)
            fsm.ChangeState(States.Dying);

    }
}
