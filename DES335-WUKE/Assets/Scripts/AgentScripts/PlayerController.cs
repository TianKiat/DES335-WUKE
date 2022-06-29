using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private Transform WeaponPivot;

    [SerializeField]
    private GameObject PeeTrail;
    [SerializeField]
    private GameObject PartyHat;
    [SerializeField]
    private HealthBar healthBar;

    private bool isFarting = false;


    // Stats...
    private static Dictionary<string, int> PlayerStats = new Dictionary<string, int>
    {
        { "stb", 5 },
        { "lcd", 5 },
        { "cog", 5 },
        { "opt", 5 }
    };

    // Runtime variables
    private float MaxHealth;
    private float CurrentHealth;
    private float CurrentMoveSpeed;
    private Rigidbody2D rb;
    private WeaponSystem weaponSystem;
    private PlayerInput playerInput;

    private Vector2 input_vec;
    private bool isFiring;
    private bool isTakingDamage;
    private float recoveryTime;
    private bool isInteract;

    public Transform avatarBody = null;

    private float holdingCoins;
    const float optCoinMultiplier = 0.03f;
    // Start is called before the first frame update
    void Awake()
    {
        // get components
        weaponSystem = GetComponent<WeaponSystem>();
        playerInput = GetComponent<PlayerInput>();

        // Resolve all stats first
        ResolveStats();

        // Initialize runtime variables
        CurrentHealth = MaxHealth;
        rb = gameObject.GetComponent<Rigidbody2D>();
        holdingCoins = 0;

        input_vec = Vector2.zero;

        // diable dreampot props
        PeeTrail.SetActive(false);
        PartyHat.SetActive(false);

        if (WeaponPivot == null)
            Debug.LogException(new System.ArgumentNullException("Assign Weapon Pivot GameObject before continuing!"));
        if (avatarBody == null)
            Debug.LogException(new System.ArgumentNullException("Body GameObject not found!"));
    }

    private void Start()
    {
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

    /*
     * Stability    ->  stb
     * Lucidity     ->  lcd
     * Cognition    ->  cog
     * Optimism     ->  opt
     */
    public int GetStat(string statName)
    {
        return PlayerStats[statName];
    }

    public void SetStat(string statName, int newValue)
    {
        PlayerStats[statName] = newValue;
        ResolveStats();
    }

    public void ModifyCurrentStat(string statName, int modValue)
    {
        PlayerStats[statName] += modValue;
        ResolveStats();
    }

    // This function must be called whenever the player stat is changed so that,
    // the stats are translated to gameplay values
    private void ResolveStats()
    {
        MaxHealth = BaseHealth + (PlayerStats["stb"] * 5);
        weaponSystem.WeaponsDamageModifier = 0.05f * PlayerStats["lcd"];
        CurrentMoveSpeed = BaseMoveSpeed + (PlayerStats["cog"] * 0.005f * BaseMoveSpeed);
        weaponSystem.ReloadSpeedModifier = 0.01f * PlayerStats["cog"];
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
        if (CurrentHealth <= 0)
            GameManager.Instance.GameOver();
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
        float multiplier = (1.0f + (PlayerStats["opt"] * optCoinMultiplier)) + (GameManager.Instance.GetLevelsCleared() * GameManager.levelClearBonus);
        holdingCoins += (amount * multiplier);

        //Update UI after this part
        
    }

    public void AddHealth(float healPercentage)
    {
        Debug.Log("Healing for " + (healPercentage * MaxHealth));
        CurrentHealth += (healPercentage * MaxHealth);
    }

    private void OnInteract(InputValue value)
    {
        isInteract = value.isPressed;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isInteract == true)
        {
            if (collision.tag == "Levelpot")
            {
                GameManager.Instance.OpenLevelPot();
            }
            //add additional tags and inputs here
        }
    }
}
