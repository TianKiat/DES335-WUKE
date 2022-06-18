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

    private bool isFarting = false;

    // Stats...
    private static Dictionary<string, int> PlayerStats = new Dictionary<string, int>
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

    private Vector2 input_vec;
    private bool isFiring;
    private bool isTakingDamage;
    private float recoveryTime;
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

        input_vec = Vector2.zero;

        // diable dreampot props
        PeeTrail.SetActive(false);
        PartyHat.SetActive(false);

        if (WeaponPivot == null)
            Debug.LogException(new System.ArgumentNullException("Assign Weapon Pivot GameObject before continuing!"));
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

        Vector2 dir = playerInput.camera.ScreenToWorldPoint(input.Get<Vector2>()) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        WeaponPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        WeaponPivot.localScale = new Vector3(1.0f, dir.x < 0 ? -1.0f : 1.0f, 1.0f);
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

    // This function must be called whenever the player stat is changed so that,
    // the stats are translated to gameplay values
    private void ResolveStats()
    {
        MaxHealth = BaseHealth + (PlayerStats["stb"] * 5);
        weaponSystem.WeaponsDamageModifier = 0.05f * PlayerStats["lcd"];
        CurrentMoveSpeed = BaseMoveSpeed + (PlayerStats["cog"] * 0.005f * BaseMoveSpeed);
        weaponSystem.ReloadSpeedModifier = 0.01f * PlayerStats["cog"];
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Player took damage, health = " + CurrentHealth);
        CurrentHealth -= damage;
        isTakingDamage = true;
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
}
