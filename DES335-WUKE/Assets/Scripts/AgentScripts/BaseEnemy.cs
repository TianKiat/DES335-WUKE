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
    [SerializeField]
    protected float coinDropAmount;

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
    [SerializeField]
    private GameObject dreamCoinDropObj;
    [SerializeField]
    private GameObject healthOrbDropObj;

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

    protected enum Tier
    {
        Grunt,
        Elite,
        MiniBoss,
        Boss,
        Secret
    }

    float[] healPercentages = new float[5] { 0.05f, 0.1f, 0.3f, 1.0f, 1.0f };
    float[] healthOrbDropChance = new float[5] { 0.2f, 0.2f, 0.3f, 1.0f, 1.0f };
    float[] healthOrbScaleSize = new float[5] { 1.0f, 1.2f, 1.4f, 2.0f, 2.0f };

    [SerializeField]
    protected Tier enemyTier;

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

        SpawnCoin();

        SpawnHealthOrb();

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

    void SpawnCoin()
    {
        //Drops a coin
        GameObject go = Instantiate(dreamCoinDropObj, transform.position, transform.rotation);
        go.GetComponent<DreamCoin>().SetAmount(coinDropAmount);
    }

    void SpawnHealthOrb()
    {
        float spawnChance = Random.Range(0.0f, 1.0f);

        //Drop different health orbs based on tier
        if (spawnChance < healthOrbDropChance[(int)enemyTier])
        {
            GameObject go = Instantiate(healthOrbDropObj, transform.position, transform.rotation);
            go.GetComponent<HealthOrb>().SetAmount(healPercentages[(int)enemyTier]);
            go.transform.localScale = new Vector3(healthOrbScaleSize[(int)enemyTier], healthOrbScaleSize[(int)enemyTier], 1.0f);
            Debug.Log("Dropping a tier " + (int)enemyTier);
        }
    }
}
