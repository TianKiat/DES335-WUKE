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
    public float knockBackForce = 5.0f;
    public float SquirmMaxRotation = 10.0f;


    private float nextSquirmTime = 0.0f;
    private float nextAttackTime = 0.0f;
    private float stunTime;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        fsm.ChangeState(States.Patrol);
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

        switch (fsm.State)
        {
            case States.Idle:
                {
                    if (Time.time > stunTime)
                        fsm.ChangeState(States.Chase);
                }
                break;
            case States.Patrol:
                {
                    if (Time.time >= nextSquirmTime)
                    {
                        nextSquirmTime = Time.time + SquirmDelay;

                        Vector2 direction = Random.insideUnitCircle.normalized;
                        rb.AddForce(direction * IdleSquirmForce, ForceMode2D.Impulse);
                        transform.localScale = new Vector3(direction.x > 0 ? 1.0f : -1.0f, 1.0f, 1.0f);
                    }
                }
                break;
            case States.Chase:
                {
                    if (Time.time >= nextSquirmTime)
                    {
                        nextSquirmTime = SquirmDelay * 0.2f + Time.time;

                        Vector3 target = (GameManager.Instance.PlayerInstance.transform.position - transform.position).normalized;
                        rb.AddForce(target * ChaseSquirmForce, ForceMode2D.Impulse);
                        transform.localScale = new Vector3(target.x > 0 ? 1.0f : -1.0f, 1.0f, 1.0f);
                    }
                }
                break;
            case States.Attack:
                {
                    if (Time.time >= nextAttackTime)
                    {
                        nextAttackTime = Time.time + AttackDelay;
                        Vector3 target = (GameManager.Instance.PlayerInstance.transform.position - transform.position).normalized;
                        GameManager.Instance.PlayerInstance.GetComponent<Rigidbody2D>().AddForce(
                            target * knockBackForce * (1.0f + DamageModifier), ForceMode2D.Impulse);
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
        {
            stunTime = Time.time + 1.0f;
            fsm.ChangeState(States.Idle);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((fsm.State != States.Dead || fsm.State != States.Dying) &&
            collision.gameObject == GameManager.Instance.PlayerInstance.gameObject)
        {
            fsm.ChangeState(States.Attack);
        }
    }
}
