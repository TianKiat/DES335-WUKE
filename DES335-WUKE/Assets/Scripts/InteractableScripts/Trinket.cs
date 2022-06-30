using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Trinket : MonoBehaviour
{
    [SerializeField]
    float stbModifier;
    [SerializeField]
    float lcdModifier;
    [SerializeField]
    float cogModifier;
    [SerializeField]
    float optModifier;

    [SerializeField]
    private float coinCost;
    private bool isShopTrinket;
    private SpriteRenderer trinketSprite;
    public bool isOnGround;

    private void Start()
    {
        trinketSprite = GetComponent<SpriteRenderer>();
    }

    public float GetStbModifier()
    {
        return stbModifier;
    }
    public float GetLcdModifier()
    {
        return lcdModifier;
    }
    public float GetCogModifier()
    {
        return cogModifier;
    }
    public float GetOptModifier()
    {
        return optModifier;
    }

    public float GetCoinCost()
    {
        return coinCost;
    }

    public bool GetIsShopTrinket()
    {
        return isShopTrinket;
    }

    public Sprite GetTrinketSprite()
    {
        return trinketSprite.sprite;
    }
}
