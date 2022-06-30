using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trinket_HUD : MonoBehaviour
{
    [SerializeField]
    Image trinketIconSprite;

    public void SetTrinketSprite(Sprite newSprite)
    {
        trinketIconSprite.sprite = newSprite;
    }
}
