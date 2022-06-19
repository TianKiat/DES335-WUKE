using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField]
    private float FireInterval = 0.1f;
    [SerializeField]
    private float DefaultBulletSpread = 1.1f;
    [SerializeField]
    private float MovementBulletSpread = 1.2f;
    [SerializeField]
    public int MagazineCapacity = 30;
    [SerializeField]
    private float ReloadTime = 1.0f;

    [SerializeField]
    private GameObject BulletPrefab = null;
    [SerializeField]
    private float DamagePerBullet = 5.0f;
    [SerializeField]
    private float BulletSpeed = 20.0f;

    public enum States
    {
        Static,
        Moving,
    }

    private StateMachine<States, StateDriverUnity> fsm;

    private float CurrentBulletSpread = 1.0f;
    private float CurrentReloadTime = 0.0f;
    private float NextFireTime;
    private bool isReloading = false;
    public int CurrentMagazineCapacity { get; private set; }
    public float WeaponDamageMod { get; set; } = 0.0f;
    public float ReloadTimeModifier { get; set; } = 0.0f;
    public float FireIntervalModifier { get; set; } = 1.0f;

    private float AdditionalReloadSpeedMod = 0.0f;

    private void Awake()
    {

        CurrentMagazineCapacity = MagazineCapacity;
        NextFireTime = FireInterval * FireIntervalModifier + Time.time;
        CurrentReloadTime = 0.0f;

        if (BulletPrefab == null)
            Debug.LogException(new System.ArgumentNullException("Assign BulletPrefab GameObject before continuing!"));

        // initialize statemachine
        fsm = new StateMachine<States, StateDriverUnity>(this);
        fsm.ChangeState(States.Static);
    }

    void Update()
    {
        if (GameManager.Instance.GetPauseState())
            return;
        // fsm.Driver.Update.Invoke();
        Reloading_Update();
    }

    // Change the state of the weapon
    public void ReloadWeapon(float addReloadSpdMod = 0.0f)
    {
        AdditionalReloadSpeedMod = addReloadSpdMod;
        Reloading_Enter();
    }

    public virtual BaseBullet FireWeapon(float addDamageMod = 0.0f)
    {
        // Fire weapon if not reloading and passed the next fire time
        if (!isReloading && Time.time >= NextFireTime)
        {
            float spread = DefaultBulletSpread;
            if (fsm.State == States.Moving)
                spread = MovementBulletSpread;
            // apply bullet spread and fire bullet
            GameObject bullet = Instantiate(BulletPrefab, transform.position, transform.rotation);
            bullet.transform.Rotate(0, 0, Random.Range(-spread, spread));
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * BulletSpeed;

            var bulletComponent = bullet.GetComponent<BaseBullet>();
            // total bullet damage = baseDamage * (1.0f + ModifierFromThisWeapon + ModifierFromPlayerStat)
            bulletComponent.Damage = DamagePerBullet * (1.0f + WeaponDamageMod + addDamageMod);
            bulletComponent.IsPlayerBullet = true;

            // spawn bullet with damage
            --CurrentMagazineCapacity;
            if (CurrentMagazineCapacity <= 0)
                Reloading_Enter();
            NextFireTime = Time.time + FireInterval;
            return bulletComponent;
        }

        return null;
    }

    private void Static_Enter()
    {
        CurrentBulletSpread = DefaultBulletSpread;
    }
    private void Moving_Enter()
    {
        CurrentBulletSpread = MovementBulletSpread;
    }
    private void Reloading_Enter()
    {
        CurrentReloadTime = 0.0f;
        isReloading = true;
    }

    private void Reloading_Update()
    {
        if (isReloading)
        {
            CurrentReloadTime += Time.deltaTime;
            if (CurrentReloadTime > ReloadTime * (1.0f - ReloadTimeModifier - AdditionalReloadSpeedMod))
                // finish reloading and transition to last state
                Reloading_Exit();
        }
    }

    private void Reloading_Exit()
    {
        CurrentReloadTime = ReloadTime;
        isReloading = false;
        CurrentMagazineCapacity = MagazineCapacity;
    }

    public bool GetIsReloading()
    {
        return isReloading;
    }

    public float GetReloadProgress()
    {
        return CurrentReloadTime / ReloadTime;
    }

    public void IsPersonMoving(bool isMoving)
    {
        if (isMoving)
            fsm.ChangeState(States.Moving);
        else
            fsm.ChangeState(States.Static);
    }
}
