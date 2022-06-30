using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class RoomTransition : MonoBehaviour
{
    [SerializeField]
    RoomTransition otherTransition;
    
    bool canTransit;

    [SerializeField]
    bool isLocked;

    SpriteRenderer objectSprite;

    [SerializeField]
    Sprite lockedSprite;
    [SerializeField]
    Sprite unlockedSprite;

    // Start is called before the first frame update
    void Awake()
    {
        objectSprite = GetComponent<SpriteRenderer>();
        canTransit = true;
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
                    collision.transform.position = otherTransition.transform.position;
                    otherTransition.SetCanTransit(false);       //Prevent constant tp-ing
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
