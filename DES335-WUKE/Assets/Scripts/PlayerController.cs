using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(WeaponSystem))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("Speed in m/s")]
    private float DefaultMoveSpeed = 4.0f;
    [SerializeField, Range(0, 1), Tooltip("Percentage modifier")]
    private float ReloadMoveSpeedModifier = 0.85f;
    [SerializeField]
    private float MaxHealth = 100.0f;

    [SerializeField]
    private Transform WeaponPivot;
    // Stats...

    // Runtime variables
    private float CurrentMoveSpeed;
    private float CurrentHealth;
    private Rigidbody2D rb;
    private WeaponSystem weaponSystem;
    private PlayerInput playerInput;

    private Vector2 input_vec;
    private bool isFiring;
    // Start is called before the first frame update
    void Awake()
    {
        // Initialize runtime variables
        CurrentMoveSpeed = DefaultMoveSpeed;
        CurrentHealth = MaxHealth;
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        weaponSystem = GetComponent<WeaponSystem>();
        playerInput = GetComponent<PlayerInput>();

        input_vec = Vector2.zero;

        

        if (WeaponPivot == null)
            Debug.LogException(new System.ArgumentNullException("Assign Weapon Pivot GameObject before continuing!"));
    }

    void Update()
    {

        weaponSystem.GetCurrentWeapon().IsPersonMoving(input_vec.sqrMagnitude >= 0.001f);


        if (weaponSystem.IsReloading())
            CurrentMoveSpeed = DefaultMoveSpeed * ReloadMoveSpeedModifier;
        else
            CurrentMoveSpeed = DefaultMoveSpeed;

        if (GameManager.Instance.GetPauseState()) return;

        // Handle movement
        rb.MovePosition(rb.position + input_vec * CurrentMoveSpeed * Time.fixedDeltaTime);

        if (isFiring)
            weaponSystem.FireWeapon();

    }

    void OnMove(InputValue input)
    {
        input_vec = input.Get<Vector2>();
    }

    void OnMousePosition(InputValue input)
    {
        
        Vector2 dir = playerInput.camera.ScreenToWorldPoint(input.Get<Vector2>()) -transform.position;
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

}
