using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GiantRatEnemy : BaseEnemy
{

    public float rotationstep = 10.0f;
    public float moveSpeed = 5.0f;

    public float knockBackForce = 10.0f;

    public float AttackDelay = 1.5f;
    public float chaseDelay = 2.5f;

    public Transform body;

    private float nextChaseTime = 0.0f;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        GameManager.Instance.RegisterEnemy(this);
        CurrentHealth = MaxHealth = BaseHealth;
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
                    if (Time.time >= nextChaseTime)
                        fsm.ChangeState(States.Chase);
                }
                break;
            case States.Patrol:
                break;
            case States.Chase:
                {
                    Vector3 target = (GameManager.Instance.PlayerInstance.transform.position - transform.position).normalized;
                    rb.velocity = target * moveSpeed;
                    body.localScale = new Vector3(target.x > 0 ? 1.0f : -1.0f, 1.0f, 1.0f);
                }
                break;
            case States.Attack:
                {
                    Vector3 target = (GameManager.Instance.PlayerInstance.transform.position - transform.position).normalized;
                    GameManager.Instance.PlayerInstance.GetComponent<Rigidbody2D>().AddForce(
                        target * knockBackForce * (1.0f + DamageModifier), ForceMode2D.Impulse);
                    GameManager.Instance.PlayerInstance.TakeDamage(Damage * (1.0f + DamageModifier));

                    fsm.ChangeState(States.Idle);
                }
                break;
            case States.Dying:
                break;
            default:
                break;
        }
    }

    void Idle_Enter()
    {
        nextChaseTime = Time.time + chaseDelay;
        rb.velocity = Vector2.zero;
    }

    protected virtual void Dying_Enter()
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

    private void OnTriggerEnter2D(Collider2D collidier)
    {
        if ((fsm.State != States.Dead || fsm.State != States.Dying || fsm.State == States.Chase) &&
            collidier.gameObject == GameManager.Instance.PlayerInstance.gameObject)
        {
            fsm.ChangeState(States.Attack);
        }
    }
}
