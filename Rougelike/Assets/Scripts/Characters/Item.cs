using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        HealthPotion,
        HPBuff,
        InfinityHpBuff,
        AttackBuff,
        SkillBuff,
        Poison,
        ManaPotion,
        RegenManaPotion,
        ManaStone, 
        Dagger,
        InfinityBag,
        PosionBag,
        InfinityAttackBuff,
        SpareBag,
        PhoenixFeather,
        BurstStone,
        DropOfFury,
        SkullOfRage,





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
            case ItemType.InfinityHpBuff:   return ItemAssets.Instance.InfinityHpBuffSprite;
            case ItemType.AttackBuff:       return ItemAssets.Instance.AttackBuffSprite;
            case ItemType.SkillBuff:        return ItemAssets.Instance.SkillBuffSprite;

            case ItemType.Poison:           return ItemAssets.Instance.PoisonSprite;
            case ItemType.ManaPotion:       return ItemAssets.Instance.ManaPotionSprite;
            case ItemType.RegenManaPotion:  return ItemAssets.Instance.RegenManaPotionSprite;
            case ItemType.ManaStone:        return ItemAssets.Instance.ManaStoneSprite;
            case ItemType.Dagger:           return ItemAssets.Instance.DaggerSprite;
            case ItemType.InfinityBag:      return ItemAssets.Instance.InfinityBagSprite;
            case ItemType.PosionBag:        return ItemAssets.Instance.PosionBagSprite;
            case ItemType.InfinityAttackBuff:   return ItemAssets.Instance.InfinityAttackBuffSprite;
            case ItemType.SpareBag:         return ItemAssets.Instance.SpareBagSprite;
            case ItemType.PhoenixFeather:   return ItemAssets.Instance.PhoenixFeatherSprite;
            case ItemType.BurstStone:       return ItemAssets.Instance.BurstStoneSprite;
            case ItemType.DropOfFury:       return ItemAssets.Instance.DropOfFurySprite;
            case ItemType.SkullOfRage:      return ItemAssets.Instance.SkullOfRageSprite;

        }
    }

    public string GetName()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion:     return "Heal Potion";
            case ItemType.HPBuff:           return "Health Buff Potion";
            case ItemType.InfinityHpBuff:   return "Health Stone";
            case ItemType.AttackBuff:       return "Damage Potion";
            case ItemType.SkillBuff:        return "Ultimate potion";

            case ItemType.Poison: return "Posion";
            case ItemType.ManaPotion: return "Mana Potion";
            case ItemType.RegenManaPotion: return "Mana Regeneration Potion ";
            case ItemType.ManaStone: return "Mana Stone";
            case ItemType.InfinityBag: return "Infinity Bag";
            case ItemType.PosionBag: return "Posion Bag";
            case ItemType.InfinityAttackBuff: return "Grindstone";
            case ItemType.SpareBag: return "Spare Bag";
            case ItemType.PhoenixFeather: return "Phoenix Feather";
            case ItemType.BurstStone: return "Burst Stone";
            case ItemType.DropOfFury: return "Drop Of Fury";
            case ItemType.SkullOfRage: return "Skull Of Rage";

        }
    }
    public string GetDescription()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion:     return "Heal 50 health point";
            case ItemType.HPBuff:           return "Gives 50 health point for max hp";
            case ItemType.InfinityHpBuff:   return "Gives 50 health point for max hp (all time)";
            case ItemType.AttackBuff:       return "Gives 15 attack damage";
            case ItemType.SkillBuff:        return "Gives 10 damage for ultimate";


            case ItemType.Poison: return "give dagger posion";
            case ItemType.ManaPotion: return "give 50 mana point";
            case ItemType.RegenManaPotion: return "Gradually restores mana";
            case ItemType.ManaStone: return "give 25 mana point (all time)";
            case ItemType.InfinityBag: return "Gradually restores daggers";
            case ItemType.PosionBag: return "give dagger posion (all time)";
            case ItemType.InfinityAttackBuff: return "give 5 atk point";
            case ItemType.SpareBag: return "+ 5 daggers";
            case ItemType.PhoenixFeather: return "fire DOT +2 atk";
            case ItemType.BurstStone: return "x2 burst mode";
            case ItemType.DropOfFury: return "+10 rage";
            case ItemType.SkullOfRage: return "give rage spell";
        }
    }


    public Color GetColor()
    {
        switch (itemType){
            default:
            case ItemType.HealthPotion: return new Color(1, 0, 0);
            case ItemType.HPBuff: return new Color(1, 0, 0);
            case ItemType.InfinityHpBuff: return new Color(0.825f, 0, 1); 
            case ItemType.AttackBuff: return new Color(0, 0.2943151f, 1);
            case ItemType.SkillBuff: return new Color(0, 0.2943151f, 1);

            case ItemType.Poison: return new Color(0.1067045f, 1, 0);
            case ItemType.ManaPotion: return new Color(0, 0.2574801f, 1);
            case ItemType.RegenManaPotion: return new Color(0, 0.2574801f, 1);
            case ItemType.ManaStone: return new Color(0.825f, 0, 1);
            case ItemType.Dagger: return new Color(0.6320754f, 0.6320754f, 0.6320754f);
            case ItemType.InfinityBag: return new Color(1, 0.4448357f, 0);
            case ItemType.PosionBag: return new Color(1, 0.4448357f, 0);
            case ItemType.InfinityAttackBuff: return new Color(0.825f, 0, 1);
            case ItemType.SpareBag: return new Color(0.825f, 0, 1);
            case ItemType.PhoenixFeather: return new Color(1, 0.4448357f, 0);
            case ItemType.BurstStone: return new Color(1, 0.4448357f, 0);
            case ItemType.DropOfFury: return new Color(0.825f, 0, 1);
            case ItemType.SkullOfRage: return new Color(1, 0.4448357f, 0);

        }


    }

    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion: 
            case ItemType.HPBuff: 
            case ItemType.AttackBuff:
            case ItemType.SkillBuff:
            case ItemType.Poison:
            case ItemType.ManaPotion:
            case ItemType.RegenManaPotion:
                return true;
            case ItemType.InfinityHpBuff:
            case ItemType.ManaStone:
            case ItemType.InfinityBag:
            case ItemType.PosionBag:
            case ItemType.InfinityAttackBuff:
            case ItemType.SpareBag:
            case ItemType.PhoenixFeather:
            case ItemType.BurstStone:
            case ItemType.DropOfFury:
            case ItemType.SkullOfRage:
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
            case ItemType.AttackBuff:
            case ItemType.SkillBuff:
            case ItemType.Poison:
            case ItemType.ManaPotion:
            case ItemType.RegenManaPotion:
                return false;
            case ItemType.InfinityHpBuff:
            case ItemType.ManaStone:
            case ItemType.InfinityBag:
            case ItemType.PosionBag:
            case ItemType.InfinityAttackBuff:
            case ItemType.SpareBag:
            case ItemType.PhoenixFeather:
            case ItemType.BurstStone:
            case ItemType.DropOfFury:
            case ItemType.SkullOfRage:
                return true;
        }
    }

    public bool IsMajorBuff()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion:
            case ItemType.HPBuff:
            case ItemType.AttackBuff:
            case ItemType.SkillBuff:
            case ItemType.Poison:
            case ItemType.ManaPotion:
            case ItemType.RegenManaPotion:
            case ItemType.InfinityHpBuff:
            case ItemType.ManaStone:
            case ItemType.InfinityAttackBuff:
            case ItemType.SpareBag:
            case ItemType.DropOfFury:
                return false;
            case ItemType.InfinityBag:
            case ItemType.PosionBag:
            case ItemType.BurstStone:
                // жрице нужен бафф
            case ItemType.SkullOfRage:
            case ItemType.PhoenixFeather:
                return true;
        }
    }

}
