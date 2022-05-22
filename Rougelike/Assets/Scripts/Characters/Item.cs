using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        HealthPotion,
        HPBuff,
        RageBuff
    }
    public ItemType itemType;
    public int amount;
    public int CD;
    public bool isCD;


    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion:     return ItemAssets.Instance.hpPotionSprite;
            case ItemType.HPBuff:           return ItemAssets.Instance.hpBuffSprite;
            case ItemType.RageBuff:         return ItemAssets.Instance.rageBuffSprite;
        }
    }


    public Color GetColor()
    {
        switch (itemType){
            default:
            case ItemType.HealthPotion: return new Color(1, 0, 0);
            case ItemType.HPBuff: return new Color(1, 0, 0);
            case ItemType.RageBuff: return new Color(1, 1, 0);
        }


    }

    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion: 
            case ItemType.HPBuff: 
            case ItemType.RageBuff:
                return true;
        }
    }

}
