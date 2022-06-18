using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseEnemy : MonoBehaviour
{

    public float BaseHealth = 100.0f;
    public float Damage = 2.0f;

    protected float CurrentHealth;
    protected float MaxHealth;
    protected float DamageModifier = 0.0f;

    protected Rigidbody2D rb;

    protected enum States
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Dying,
        Dead
    }

    protected StateMachine<States, StateDriverUnity> fsm;

    // Start is called before the first frame update
    void Awake()
    {
        CurrentHealth = MaxHealth = BaseHealth;
        rb = GetComponent<Rigidbody2D>();
        fsm = new StateMachine<States, StateDriverUnity>(this);
        fsm.ChangeState(States.Idle);
    }

    public virtual void Start()
    {
        GameManager.Instance.RegisterEnemy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Dead_Enter()
    {
        //de register from enemy manager pool
        GameManager.Instance.DeregisterEnemy(this);
        Destroy(gameObject);
    }

    public virtual void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
    }

    public virtual void SetDamageModifier(float mod)
    {
        DamageModifier = mod;
    }
}
