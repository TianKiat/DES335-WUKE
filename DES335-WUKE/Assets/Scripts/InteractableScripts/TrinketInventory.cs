using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinketInventory : MonoBehaviour
{
    [SerializeField]
    private Trinket[] TrinketBag = new Trinket[1];
    private int currentTrinketIndex;
    private int numCurrentTrinkets = 0;

    public void CycleTrinketLeft()
    {
        --currentTrinketIndex;
        
        //Wrapping scroll
        if (currentTrinketIndex < 0)
        {
            bool setIndex = false;

            //Take the next highest index
            for (int i = TrinketBag.Length - 1; i > 0; --i)
            {
                if (TrinketBag[i] != null)
                {
                    currentTrinketIndex = i;
                    setIndex = true;
                    UpdatePlayerStat();
                    HideInactiveTrinkets();
                }
            }

            if (!setIndex)
            {
                ++currentTrinketIndex;
            }
        }
        //Normal scroll
        else
        {
            if (TrinketBag[currentTrinketIndex] != null)
            {
                UpdatePlayerStat();
                HideInactiveTrinkets();
            }
            else
            {
                ++currentTrinketIndex;
            }
        }
    }

    public void CycleTrinketRight()
    {
        ++currentTrinketIndex;

        //Wrapping scroll
        if (currentTrinketIndex > numCurrentTrinkets - 1)
        {
            currentTrinketIndex = 0;

            if (TrinketBag[currentTrinketIndex] != null)
            {
                UpdatePlayerStat();
                HideInactiveTrinkets();
            }
        }
        //Normal scroll
        else
        {
            if (TrinketBag[currentTrinketIndex] != null)
            {
                UpdatePlayerStat();
                HideInactiveTrinkets();
            }
            else
            {
                --currentTrinketIndex;
            }
        }
    }

    public void UpdatePlayerStat()
    {
        //Additional Stb
        float finalStb = GameManager.Instance.PlayerInstance.GetStat("stb") * TrinketBag[currentTrinketIndex].GetStbModifier();
        GameManager.Instance.PlayerInstance.SetAddStat("stb", (int)finalStb);

        //Additional Lcd
        float finalLcd = GameManager.Instance.PlayerInstance.GetStat("lcd") * TrinketBag[currentTrinketIndex].GetLcdModifier();
        GameManager.Instance.PlayerInstance.SetAddStat("lcd", (int)finalLcd);

        //Additional Cog
        float finalCog = GameManager.Instance.PlayerInstance.GetStat("cog") * TrinketBag[currentTrinketIndex].GetCogModifier();
        GameManager.Instance.PlayerInstance.SetAddStat("cog", (int)finalCog);

        //Additional Opt
        float finalOpt = GameManager.Instance.PlayerInstance.GetStat("opt") * TrinketBag[currentTrinketIndex].GetOptModifier();
        GameManager.Instance.PlayerInstance.SetAddStat("opt", (int)finalOpt);

        HUD_Manager.Instance.UpdateHUD();
    }

    public void AddTrinketToInventory(Trinket trinket, ref float playerCurrency)
    {
        //Check if trinket has a price and if holding enough currency
        if (trinket.GetIsShopTrinket() && playerCurrency > trinket.GetCoinCost())
        {
            bool addNewTrinket = false;

            for (int i = 0; i < TrinketBag.Length; ++i)
            {
                if (TrinketBag[i] == null)
                {
                    addNewTrinket = true;
                    AddTrinketToInventory(trinket, i);
                    break;
                }
            }

            //Swap out currently active trinket with pick up
            if (!addNewTrinket)
            {
                SwapFromGround(trinket);
            }
        }
        //Enemy drop
        else
        {
            bool addNewTrinket = false;

            for (int i = 0; i < TrinketBag.Length; ++i)
            {
                if (TrinketBag[i] == null)
                {
                    addNewTrinket = true;
                    AddTrinketToInventory(trinket, i);
                    break;
                }
            }

            //Swap out currently active trinket with pick up
            if (!addNewTrinket)
            {
                SwapFromGround(trinket);
            }
        }
    }

    void AddTrinketToInventory(Trinket trinket, int index)
    {
        trinket.isOnGround = false;
        TrinketBag[index] = trinket;
        ResetTransform(trinket);
        HUD_Manager.Instance.GetTrinketHUD().SetTrinketSprite(trinket.GetTrinketSprite());
        trinket.GetComponent<SpriteRenderer>().enabled = false;
        UpdatePlayerStat();
        ++numCurrentTrinkets;
    }

    void SwapFromGround(Trinket trinket)
    {
        //Detach current trinket from player and drop on ground
        TrinketBag[currentTrinketIndex].gameObject.transform.parent = null;
        TrinketBag[currentTrinketIndex].isOnGround = true;
        TrinketBag[currentTrinketIndex].GetComponent<BoxCollider2D>().enabled = true;
        TrinketBag[currentTrinketIndex].GetComponent<SpriteRenderer>().enabled = true;

        //Add new trinket to current slot
        trinket.isOnGround = false;
        trinket.GetComponent<BoxCollider2D>().enabled = false;
        trinket.GetComponent<SpriteRenderer>().enabled = false;
        TrinketBag[currentTrinketIndex] = trinket;
        ResetTransform(trinket);
        HUD_Manager.Instance.GetTrinketHUD().SetTrinketSprite(trinket.GetTrinketSprite());
        UpdatePlayerStat();
    }

    void ResetTransform(Trinket trinket)
    {
        trinket.gameObject.transform.parent = GetComponent<PlayerController>().GetTrinketTransform();
        trinket.gameObject.transform.localPosition = Vector3.zero;
        trinket.gameObject.transform.localScale = new Vector3(-0.5f, 0.5f, 1.0f);
        HideInactiveTrinkets();
    }

    void HideInactiveTrinkets()
    {
        for (int i = 0; i < TrinketBag.Length; ++i)
        {
            if (TrinketBag[i] != null)
            {
                if (i != currentTrinketIndex)
                {
                    TrinketBag[i].gameObject.SetActive(false);
                }
                else
                {
                    TrinketBag[i].gameObject.SetActive(true);
                }
            }
        }
    }
}
