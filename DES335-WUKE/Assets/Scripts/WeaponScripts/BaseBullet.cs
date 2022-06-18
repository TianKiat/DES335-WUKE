using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public float Damage { get; set; }
    public bool IsPlayerBullet { get; set; }


    private float lifeTime = 10.0f;
    private float currentLifeTime = 0.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnHitResponse(other.gameObject);
    }

    public virtual void OnHitResponse(GameObject other)
    {
        // Deal Damage here
        if (IsPlayerBullet)
        {
            BaseEnemy baseEnemy = other.GetComponent<BaseEnemy>();
            // only deal damage to enemy types
            if (baseEnemy != null)
                baseEnemy.TakeDamage(Damage);
        }
        else if (other == GameManager.Instance.PlayerInstance.gameObject)
        {
            GameManager.Instance.PlayerInstance.TakeDamage(Damage);
        }

        other.GetComponent<Rigidbody2D>().AddForce(
                            transform.right * Damage, ForceMode2D.Impulse);
    }

    private void Update()
    {
        currentLifeTime += Time.deltaTime;
        if (currentLifeTime >= lifeTime)
            Destroy(this);
    }
}
