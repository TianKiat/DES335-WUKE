using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseEnemy : MonoBehaviour
{
    [SerializeField]
    protected float BaseHealth = 100.0f;
    [SerializeField]
    protected float Damage = 2.0f;

    protected float CurrentHealth;
    protected float MaxHealth;
    protected float DamageModifier = 0.0f;

    protected Rigidbody2D rb;

    [SerializeField]
    private GameObject PartyHat;
    [SerializeField]
    private HealthBar healthbar;
    [SerializeField]
    private GameObject buffIcon;
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
        PartyHat.SetActive(false);
    }

    // Update is called once per frame
    protected virtual void Update()
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
        healthbar.UpdateHealth(CurrentHealth / MaxHealth);
    }

    public virtual void SetDamageModifier(float mod)
    {
        DamageModifier = mod;
    }

    public void ApplyDOT(IEnumerator DOTCoroutine)
    {
        StartCoroutine(DOTCoroutine);
    }

    public void ActivateHat()
    {
        PartyHat.SetActive(true);
    }

    public void Activatejapanese()
    {
        healthbar.gameObject.SetActive(false);
    }

    public void ActivateGymRat()
    {
        DamageModifier += 0.1f;

        // add buff icon
        buffIcon.SetActive(true);
    }

    public float GetMaxHealth()
    {
        return MaxHealth;
    }
}