using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public float Damage { get; set; }
    public bool IsPlayerBullet { get; set; }


    private float lifeTime = 5.0f;
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
            {
                baseEnemy.TakeDamage(Damage);
                Destroy(gameObject);
                AddKnockback(other);
            }

        }
        else if (other == GameManager.Instance.PlayerInstance.gameObject)
        {
            GameManager.Instance.PlayerInstance.TakeDamage(Damage);
            Destroy(gameObject);
            AddKnockback(other);
        }

    }

    protected void AddKnockback(GameObject other)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            other.GetComponent<Rigidbody2D>().AddForce(rb.velocity * 0.75f, ForceMode2D.Impulse);
    }

    private void Update()
    {
        currentLifeTime += Time.deltaTime;
        if (currentLifeTime >= lifeTime)
            Destroy(gameObject);
    }
}
