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

        Debug.Log("Registered " + enemy.gameObject.name);
    }

    public void DeregisterEnemy(BaseEnemy enemy)
    {
        activeEnemies.Remove(enemy);
        Debug.Log("Deregistered " + enemy.gameObject.name);
    }

}
