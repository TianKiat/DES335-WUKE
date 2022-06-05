using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Vector2 input_vec;
    // Start is called before the first frame update
    void Awake()
    {
        // Initialize runtime variables
        CurrentMoveSpeed = DefaultMoveSpeed;
        CurrentHealth = MaxHealth;
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        weaponSystem = GetComponent<WeaponSystem>();

        input_vec = Vector2.zero;

        if (WeaponPivot == null)
            Debug.LogException(new System.ArgumentNullException("Assign Weapon Pivot GameObject before continuing!"));
    }

    void Update()
    {
        // capture input for movement
        input_vec.x = Input.GetAxis("Horizontal");
        input_vec.y = Input.GetAxis("Vertical");
        input_vec.Normalize();

        // call weapon system for shooting
        if (Input.GetButton("Fire1"))
            weaponSystem.FireWeapon();

        if (Input.GetButtonDown("Reload"))
            weaponSystem.ReloadWeapon();

        weaponSystem.GetCurrentWeapon().IsPersonMoving(input_vec.sqrMagnitude >= 0.001f);

        UpdateWeaponPivot();

        if (weaponSystem.IsReloading())
            CurrentMoveSpeed = DefaultMoveSpeed * ReloadMoveSpeedModifier;
        else
            CurrentMoveSpeed = DefaultMoveSpeed;
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        rb.MovePosition(rb.position + input_vec * CurrentMoveSpeed * Time.fixedDeltaTime);
    }

    void UpdateWeaponPivot()
    {
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(WeaponPivot.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        WeaponPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
