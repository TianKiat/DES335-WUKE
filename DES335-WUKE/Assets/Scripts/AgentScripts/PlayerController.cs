using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(WeaponSystem))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float BaseHealth = 100.0f;
    [SerializeField, Tooltip("Speed in m/s")]
    private float BaseMoveSpeed = 4.0f;
    [SerializeField, Range(0, 1), Tooltip("Percentage modifier")]
    private float ReloadMoveSpeedModifier = 0.85f;
    [SerializeField]
    private float DamageRecoveryTime = 0.85f;
    [SerializeField]
    public WeaponSystem WeaponSystem;

    [SerializeField]
    private Transform WeaponPivot;
    [SerializeField]
    private Transform TrinketPoint;

    [SerializeField]
    private GameObject PeeTrail;
    [SerializeField]
    private GameObject PartyHat;
    [SerializeField]
    private HealthBar healthBar;

    private bool isFarting = false;


    // Stats...
    private static Dictionary<string, int> BaseStats = new Dictionary<string, int>
    {
        { "stb", 5 },
        { "lcd", 5 },
        { "cog", 5 },
        { "opt", 5 }
    };

    private static Dictionary<string, int> AddStats = new Dictionary<string, int>
    {
        { "stb", 0 },
        { "lcd", 0 },
        { "cog", 0 },
        { "opt", 0 }
    };

    // Runtime variables
    private float MaxHealth;
    private float CurrentHealth;
    private float CurrentMoveSpeed;
    private Rigidbody2D rb;
    private WeaponSystem weaponSystem;
    private PlayerInput playerInput;
    private TrinketInventory trinketInventory;

    private Vector2 input_vec;
    private bool isFiring;
    private bool isTakingDamage;
    private float recoveryTime;
    private bool isInteract;
    private bool isInteractDrop;

    public Transform avatarBody = null;

    private float holdingCoins;
    const float optCoinMultiplier = 0.03f;

    private GameObject collidedDrop;

    // Start is called before the first frame update
    void Awake()
    {
        // get components
        weaponSystem = GetComponent<WeaponSystem>();
        trinketInventory = GetComponent<TrinketInventory>();
        playerInput = GetComponent<PlayerInput>();

        // diable dreampot props
        PeeTrail.SetActive(false);
        PartyHat.SetActive(false);

        if (WeaponPivot == null)
            Debug.LogException(new System.ArgumentNullException("Assign Weapon Pivot GameObject before continuing!"));
        if (avatarBody == null)
            Debug.LogException(new System.ArgumentNullException("Body GameObject not found!"));

        // Resolve all stats first
        ResolveStats();

        // Initialize runtime variables
        CurrentHealth = MaxHealth;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        holdingCoins = 0;

        input_vec = Vector2.zero;
    }

    void Update()
    {
        weaponSystem.GetCurrentWeapon().IsPersonMoving(input_vec.sqrMagnitude >= 0.001f);


        if (weaponSystem.IsReloading())
            CurrentMoveSpeed = BaseMoveSpeed * ReloadMoveSpeedModifier;
        else
            CurrentMoveSpeed = BaseMoveSpeed;

        if (GameManager.Instance.GetPauseState()) return;

        // Handle movement
        if (!isTakingDamage)
        {
            rb.MovePosition(rb.position + input_vec * CurrentMoveSpeed * Time.fixedDeltaTime);

            if (input_vec.x == -1.0f && avatarBody.localScale.x == 0.5f || input_vec.x == 1.0f && avatarBody.localScale.x == -0.5f)
            {
                avatarBody.localScale = new Vector3(-avatarBody.localScale.x,
                   avatarBody.localScale.y, avatarBody.localScale.z);
            }

        }
        else
        {
            recoveryTime += Time.deltaTime;
            if (recoveryTime >= DamageRecoveryTime)
            {
                isTakingDamage = false;
                recoveryTime = 0.0f;
            }
        }

        if (isFiring)
            weaponSystem.FireWeapon();
    }

    void OnMove(InputValue input)
    {
        input_vec = input.Get<Vector2>();
    }

    void OnMousePosition(InputValue input)
    {

        Vector2 dir = (playerInput.camera.ScreenToWorldPoint(input.Get<Vector2>()) - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        WeaponPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        WeaponPivot.localScale = new Vector3(1.0f, dir.x > 0 ? 1.0f : -1.0f, 1.0f);
    }

    void OnFire(InputValue input)
    {
        isFiring = input.isPressed;
    }

    void OnReload()
    {
        weaponSystem.ReloadWeapon();
    }

    void OnSwitchWeapon()
    {
        weaponSystem.SwitchWeapon();
    }

    void OnCycleTrinketLeft()
    {
        trinketInventory.CycleTrinketLeft();
    }

    void OnCycleTrinketRight()
    {
        trinketInventory.CycleTrinketRight();
    }

    /*
     * Stability    ->  stb
     * Lucidity     ->  lcd
     * Cognition    ->  cog
     * Optimism     ->  opt
     */
    public int GetStat(string statName)
    {
        return BaseStats[statName];
    }

    public void SetStat(string statName, int newValue)
    {
        BaseStats[statName] = newValue;
        ResolveStats();
    }

    public void SetAddStat(string statName, int newValue)
    {
        AddStats[statName] = newValue;
        ResolveStats();
    }

    public void ModifyCurrentStat(string statName, int modValue)
    {
        BaseStats[statName] += modValue;
        ResolveStats();
    }

    // This function must be called whenever the player stat is changed so that,
    // the stats are translated to gameplay values
    private void ResolveStats()
    {
        MaxHealth = BaseHealth + ((BaseStats["stb"] + AddStats["stb"]) * 5);
        weaponSystem.WeaponsDamageModifier = 0.05f * (BaseStats["lcd"] + AddStats["lcd"]);
        CurrentMoveSpeed = BaseMoveSpeed + ((BaseStats["cog"] + AddStats["cog"]) * 0.005f * BaseMoveSpeed);
        weaponSystem.ReloadSpeedModifier = 0.01f * (BaseStats["cog"] + AddStats["cog"]);
    }

    public void TakeDamage(float damage, bool isProjectile = true)
    {
        CurrentHealth -= damage;

        healthBar.UpdateHealth(CurrentHealth / MaxHealth);

        if (isProjectile)
        {
            isTakingDamage = true;
        }

        if (isFarting)
        {
            // play farting noise and vfx
        }
        if (CurrentHealth <= 0) ;
            //GameManager.Instance.GameOver();
    }

    public void ActivatePee()
    {
        PeeTrail.SetActive(true);
    }
    public void ActivateHat()
    {
        PartyHat.SetActive(true);
    }

    public void ActivateFart()
    {
        isFarting = true;
    }

    public void Activatejapanese()
    {
        healthBar.gameObject.SetActive(false);
    }

    public float GetMaxHealth()
    {
        return MaxHealth;
    }

    public void AddCoins(float amount)
    {
        //Multiplier based on opt stat and level cleared
        float multiplier = (1.0f + (BaseStats["opt"] * optCoinMultiplier)) + (GameManager.Instance.GetLevelsCleared() * GameManager.levelClearBonus);
        holdingCoins += (amount * multiplier);

        //Update UI after this part

    }

    public void SubtractCoin(float amount)
    {
        holdingCoins -= amount;
    }

    public float GetHoldingCoins()
    {
        return holdingCoins;
    }

    public void AddHealth(float healPercentage)
    {
        Debug.Log("Healing for " + (healPercentage * MaxHealth));
        CurrentHealth += (healPercentage * MaxHealth);
    }

    private void OnInteract()
    {
        if (isInteract)
            GameManager.Instance.OpenLevelPot();
        if (isInteractDrop)
        {
            //Pick weapon
            if (collidedDrop.GetComponent<BaseWeapon>() != null)
            {
                if (collidedDrop.GetComponent<BaseWeapon>().isOnGround)
                {
                    weaponSystem.PickUp(collidedDrop.GetComponent<BaseWeapon>(), ref holdingCoins);
                    isInteractDrop = false;
                }
            }
            //Pick trinket
            else if (collidedDrop.GetComponent<Trinket>() != null)
            {
                if (collidedDrop.GetComponent<Trinket>().isOnGround)
                {
                    trinketInventory.AddTrinketToInventory(collidedDrop.GetComponent<Trinket>(), ref holdingCoins);
                    isInteractDrop = false;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Levelpot")
        {
            isInteract = true;
        }
        //add additional tags and inputs here
        if (collision.tag == "Weapon")
        {
            collidedDrop = collision.gameObject;
            isInteractDrop = true;
        }
        if (collision.tag == "Trinket")
        {
            collidedDrop = collision.gameObject;
            isInteractDrop = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == collidedDrop)
        {
            collidedDrop = null;
            isInteractDrop = false; 
        }
        if (collision.gameObject == collidedDrop)
        {
            collidedDrop = null;
            isInteractDrop = false;
        }
        if (collision.tag == "Levelpot")
        {
            isInteract = false;
        }
    }

    public void SetCurrentHealth(float health)
    {
        CurrentHealth = health;
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnLevelLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnLevelLoaded;
    }

    private void OnLevelLoaded(Scene scene)
    {
        foreach (var stat in BaseStats)
        {
            GameManager.Instance.SetStat(stat.Key, stat.Value);
        }
        GameManager.Instance.SetCurrentHealth(CurrentHealth);
    }
    public Transform GetWeaponPivot()
    {
        return WeaponPivot.transform;
    }

    public Transform GetTrinketTransform()
    {
        return TrinketPoint.transform;
    }
}
