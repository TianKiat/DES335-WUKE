using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    bool canTransit;

    [SerializeField]
    string levelToLoad;

    [SerializeField]
    bool isLocked;

    SpriteRenderer objectSprite;

    [SerializeField]
    Sprite lockedSprite;
    [SerializeField]
    Sprite unlockedSprite;

    // Start is called before the first frame update
    void Start()
    {
        canTransit = true;
        objectSprite = GetComponent<SpriteRenderer>();
        SetLock(isLocked);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLocked)
        {
            if (collision.tag == "Player")
            {
                if (canTransit)
                {
                    GameManager.Instance.ClearEnemyList();
                    SceneManager.LoadScene(levelToLoad);
                }
            }
        }
    }

    public void SetCanTransit(bool toggle)
    {
        canTransit = toggle;
    }

    public void SetLock(bool toggle)
    {
        isLocked = toggle;

        if (isLocked)
        {
            objectSprite.sprite = lockedSprite;
        }
        else
        {
            objectSprite.sprite = unlockedSprite;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!canTransit)
        {
            canTransit = true;
        }
    }
}
