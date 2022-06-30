using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool isRoomCleared;
    public GameObject[] RoomEnemies;
    public RoomTransition[] DoorsToUnlock;
    public LevelTransition[] levelTransitionsToUnlock;

    private bool doorsUnlocked = false;
    private bool enteredRoom = false;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject enemy in RoomEnemies)
        {
            enemy.SetActive(false);
        }
        foreach (RoomTransition door in DoorsToUnlock)
        {
            door.SetLock(true);
        }
        foreach (LevelTransition door in levelTransitionsToUnlock)
        {
            door.SetLock(true);
        }
    }

    private void FixedUpdate()
    {
        if(!isRoomCleared)
        {
            bool isClear = true;
            for (int i = 0; i < RoomEnemies.Length; i++)
            {
                if (RoomEnemies[i] != null)
                {
                    isClear = false;

                }
            }
            isRoomCleared = isClear;
        }

        if (isRoomCleared && !doorsUnlocked)
        {
            foreach (RoomTransition door in DoorsToUnlock)
            {
                door.SetLock(false);
            }
            foreach (LevelTransition door in levelTransitionsToUnlock)
            {
                door.SetLock(false);
            }
            doorsUnlocked = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!doorsUnlocked && !isRoomCleared && !enteredRoom && collision.gameObject.CompareTag("Player"))
        {
            enteredRoom = true;
            foreach (GameObject enemy in RoomEnemies)
            {
                //if (enemy != null)
                    enemy.SetActive(true);
            }
        }
    }
}
