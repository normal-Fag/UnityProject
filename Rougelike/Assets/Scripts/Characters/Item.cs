using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        HealthPotion,
        HPBuff,
        RageBuff,
        InfinityHpBuff,
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
            case ItemType.InfinityHpBuff:   return ItemAssets.Instance.InfinityHpBuffSprite;
        }
    }

    public string GetName()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion: return "Heal Potion";
            case ItemType.HPBuff: return "Health Buff Potion";
            case ItemType.RageBuff: return "Rage Potion";
            case ItemType.InfinityHpBuff: return "Health Stone";
        }
    }
    public string GetDescription()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion: return "Heal 50 health point";
            case ItemType.HPBuff: return "Gives 50 health point for max hp";
            case ItemType.RageBuff: return "Test";
            case ItemType.InfinityHpBuff: return "Gives 50 health point for max hp (all time)";
        }
    }


    public Color GetColor()
    {
        switch (itemType){
            default:
            case ItemType.HealthPotion: return new Color(1, 0, 0);
            case ItemType.HPBuff: return new Color(1, 0, 0);
            case ItemType.RageBuff: return new Color(1, 1, 0);
            case ItemType.InfinityHpBuff: return new Color(0.825f, 0, 1);
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
            case ItemType.InfinityHpBuff:
                return false;
        }
    }

    public bool IsInfinityBuff()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion:
            case ItemType.HPBuff:
            case ItemType.RageBuff:
                return false;
            case ItemType.InfinityHpBuff:
                return true;
        }
    }

}
