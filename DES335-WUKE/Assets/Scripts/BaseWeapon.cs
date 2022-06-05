using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField]
    private float FireInterval = 0.1f;
    [SerializeField]
    private float DamagePerBullet = 5.0f;
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

    public enum States
    {
        Static,
        Moving,
        Reloading
    }

    private StateMachine<States, StateDriverUnity> fsm;

    private float CurrentBulletSpread = 1.0f;
    private float CurrentReloadTime = 0.0f;
    private float NextFireTime;
    private bool isReloading = false;
    public int CurrentMagazineCapacity { get; private set; }

    private void Awake()
    {

        CurrentMagazineCapacity = MagazineCapacity;
        NextFireTime = Time.time + FireInterval;
        CurrentReloadTime = 0.0f;

        // initialize statemachine
        fsm = new StateMachine<States, StateDriverUnity>(this);
        fsm.ChangeState(States.Static);
    }

    void Update()
    {
        fsm.Driver.Update.Invoke();
    }

    // Change the state of the weapon
    public void ReloadWeapon()
    {
            fsm.ChangeState(States.Reloading);
    }

    public void IsPersonMoving(bool isMoving)
    {
        if (isMoving)
            fsm.ChangeState(States.Moving);
        else
            fsm.ChangeState(States.Static);
    }

    public virtual void FireWeapon()
    {
        if (Time.time >= NextFireTime)
        {
            // apply bullet spread

            // spawn bullet with damage
            Debug.Log("Fired Bullet");
            --CurrentMagazineCapacity;

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
        Debug.Log("Reloading");
        CurrentReloadTime += Time.deltaTime;
        if (CurrentReloadTime > ReloadTime)
        {
            CurrentMagazineCapacity = MagazineCapacity;

            // finish reloading and transition to last state
            fsm.ChangeState(fsm.LastState);
        }
    }

    private void Reloading_Exit()
    {
        Debug.Log("Finished Reload");
        CurrentReloadTime = ReloadTime;
        isReloading = false;
    }

    public bool GetIsReloading()
    {
        return isReloading;
    }

    public float GetReloadProgress()
    {
        return CurrentReloadTime / ReloadTime;
    }
}
