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
    }

    public ItemType itemType;
    public int amount;
}
