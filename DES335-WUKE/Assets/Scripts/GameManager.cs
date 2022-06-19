using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool isGamePaused = false;
    [SerializeField]
    public PlayerController PlayerInstance = null;

    List<BaseEnemy> activeEnemies = new List<BaseEnemy>();
    private void Awake()
    {

        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPauseState(bool isPause)
    {
        isGamePaused = isPause;
    }

    public bool GetPauseState()
    {
        return isGamePaused;
    }

    public void GameOver()
    {
        SetPauseState(true);
        Debug.Log("Game Over");
    }

    public void RegisterEnemy(BaseEnemy enemy)
    {
        activeEnemies.Add(enemy);
    }

    public void DeregisterEnemy(BaseEnemy enemy)
    {
        activeEnemies.Remove(enemy);
    }

    public void SetDreamPotEffect(int index)
    {
        switch (index)
        {
            case 0:
                {
                    PlayerInstance.ActivateHat();
                    foreach (BaseEnemy enemy in activeEnemies)
                    {
                        enemy.ActivateHat();
                    }
                }
                break;
            case 1:
                {
                    PlayerInstance.ActivatePee();
                }
                break;
            case 2:
                {
                    PlayerInstance.ActivateFart();
                }
                break;
            case 3:
                //FoolsGold();
                break;
            case 4:
                //Epiphany();
                break;
            case 5:
                {
                    // weapons do 20% more damage
                    PlayerInstance.GetComponent<WeaponSystem>().WeaponsDamageModifier += 0.2f;
                }
                break;
            case 6:
                //GreedyGrubs();
                break;
            case 7:
                {
                    foreach (BaseEnemy enemy in activeEnemies)
                    {
                        enemy.ActivateGymRat();
                    }
                }
                break;
            case 8:
                {
                    PlayerInstance.Activatejapanese();
                    foreach (BaseEnemy enemy in activeEnemies)
                    {
                        enemy.Activatejapanese();
                    }
                }
                break;
            default:
                Debug.Log("This dreampot index is not supported!");
                break;
        }
    }

}
