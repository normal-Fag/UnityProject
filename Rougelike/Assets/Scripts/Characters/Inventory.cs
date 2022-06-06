using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChanged;
    private List<Item> itemList;
    private Action<Item, int> useItemAction;
    public int max_stack = 5;

    public Inventory(Action<Item, int> useItemAction)
    {
        this.useItemAction = useItemAction;
        itemList = new List<Item>();
   
    }

    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {

            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType && inventoryItem.amount < max_stack)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                    break;
                }
            }
            if (!itemAlreadyInInventory)
            {
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

    public void UseItem(Item item, int index)
    {
        useItemAction(item, index);
    }
    public void RemoveItem(Item item, int index)
    {
        if (item.IsStackable())
        {

            Item itemInInventory = null;

            if (item.itemType == itemList[index].itemType)
            {
                itemList[index].amount -= 1;
                itemInInventory = itemList[index];
            }
                 
            
            if (itemList[index].amount <= 0)
            {
                itemList.Remove(itemInInventory);
            }
        }
        else
        {
            itemList.Remove(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    
   

    }
}

