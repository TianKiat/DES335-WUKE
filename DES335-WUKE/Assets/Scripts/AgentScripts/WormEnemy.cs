using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WormEnemy : BaseEnemy
{
    public float IdleSquirmForce = 2.0f;
    public float ChaseSquirmForce = 5.0f;
    public float SquirmDelay = 1.0f;
    public float AttackDelay = 0.5f;
    public float knockBackForce = 4.0f;
    public float SquirmMaxRotation = 10.0f;


    private float nextSquirmTime = 0.0f;
    private float nextAttackTime = 0.0f;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        CurrentHealth = MaxHealth = BaseHealth = 10.0f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (GameManager.Instance.GetPauseState()) return;

        switch (fsm.State)
        {
            case States.Idle:
                {
                    fsm.ChangeState(States.Patrol);
                }
                break;
            case States.Patrol:
                {
                    if (Time.time >= nextSquirmTime)
                    {
                        nextSquirmTime = Time.time + SquirmDelay;

                        transform.Rotate(0, 0, Random.Range(-SquirmMaxRotation, SquirmMaxRotation));
                        rb.AddForce(transform.right * IdleSquirmForce, ForceMode2D.Impulse);
                    }
                }
                break;
            case States.Chase:
                {
                    if (Time.time >= nextSquirmTime)
                    {
                        nextSquirmTime = SquirmDelay * 0.2f + Time.time;
                        Vector3 target = GameManager.Instance.PlayerInstance.transform.position - transform.position;

                        transform.right = Vector3.Slerp(transform.right, (target), SquirmMaxRotation * 0.5f);

                        rb.AddForce(transform.right * ChaseSquirmForce, ForceMode2D.Impulse);
                    }
                }
                break;
            case States.Attack:
                {
                    if (Time.time >= nextAttackTime)
                    {
                        nextAttackTime = Time.time + AttackDelay;
                        GameManager.Instance.PlayerInstance.GetComponent<Rigidbody2D>().AddForce(
                            transform.right * knockBackForce * (1.0f + DamageModifier), ForceMode2D.Impulse);
                        GameManager.Instance.PlayerInstance.TakeDamage(Damage * (1.0f + DamageModifier));
                    }
                    else if (fsm.State != States.Chase)
                    {
                        fsm.ChangeState(States.Chase);
                    }
                }
                break;
            case States.Dying:
                break;
            default:
                break;
        }

        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
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
        else if (fsm.State != States.Chase)
            fsm.ChangeState(States.Chase);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((fsm.State != States.Dead || fsm.State != States.Dying) &&
            collision.gameObject == GameManager.Instance.PlayerInstance.gameObject)
        {
            fsm.ChangeState(States.Attack);
        }
    }
}
