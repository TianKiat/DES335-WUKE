using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpikeTrap : ParentTraps
{
    
    void Start()
    {
        //Init the values here on start
        trapPlayerDamage = 10;
        trapEnemyDamage = 4;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            //Do more damage to player
            case "Player":
                PlayerController pc = collision.GetComponent<PlayerController>();
                pc.TakeDamage(pc.GetMaxHealth() * (1 / trapPlayerDamage), false);
                break;
            //Do less damage to enemy
            case "Enemy":
                BaseEnemy enemy = collision.GetComponent<BaseEnemy>();
                enemy.TakeDamage(enemy.GetMaxHealth() * (1 / trapEnemyDamage));
                break;
        }
    }
}
