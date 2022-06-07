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
    private int MagazineCapacity = 30;
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
    public float DamagePerBulletModifier { get; set; } = 1.0f;
    public float ReloadTimeModifier { get; set; } = 1.0f;
    public float FireIntervalModifier { get; set; } = 1.0f;

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
        // fsm.Driver.Update.Invoke();
        Reloading_Update();
    }

    // Change the state of the weapon
    public void ReloadWeapon()
    {
        Reloading_Enter();
    }

    public virtual void FireWeapon()
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
            bulletComponent.Damage = DamagePerBullet * DamagePerBulletModifier;
            bulletComponent.IsPlayerBullet = true;

            // spawn bullet with damage
            --CurrentMagazineCapacity;
            Debug.Log("Fired Bullet, magazine now at: " + CurrentMagazineCapacity + "/" + MagazineCapacity);
            if (CurrentMagazineCapacity <= 0)
                Reloading_Enter();
            NextFireTime = Time.time + FireInterval;
        }
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
        Debug.Log("Started Reload");
        isReloading = true;
    }

    private void Reloading_Update()
    {
        if (isReloading)
        {
            CurrentReloadTime += Time.deltaTime;
            Debug.Log("Reloading: " + GetReloadProgress() + "% done.");
            if (CurrentReloadTime > ReloadTime * ReloadTimeModifier)
                // finish reloading and transition to last state
                Reloading_Exit();
        }
    }

    private void Reloading_Exit()
    {
        Debug.Log("Finished Reload");
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
