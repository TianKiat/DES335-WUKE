using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool isGamePaused = false;
    [SerializeField]
    public PlayerController PlayerInstance = null;


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

}
