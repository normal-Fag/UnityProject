using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    public Transform itemSlotContainder;
    public Transform itemSlot;
    public Transform itemCDContainder;
    public Transform itemCD;
    private character_movement character;
    Vector3 checkPos;

    private bool isTouched;

    private void Awake()
    {
        itemSlotContainder = transform.Find("ItemSlotContainer");
        itemSlot = itemSlotContainder.Find("ItemSlot");
        itemCDContainder = transform.Find("CDSlotContainer"); 
        itemCD = itemCDContainder.Find("CD");
}


    public void SetCharacter(character_movement character)
    {
        this.character = character;
    }
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }
    private void RefreshInventoryItems()
    {
        foreach(Transform child in itemSlotContainder)
        {
            if (child == itemSlot) continue;
            else Destroy(child.gameObject);
        }
        int x = 0;

        float itemSize = 60f;

        foreach (Item item in inventory.GetItemList())
        {
            isTouched = false;
            Transform itemSlotRectTransform = Instantiate(itemSlot, itemSlotContainder).GetComponent<Transform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            itemSlotRectTransform.position = new Vector2(itemSlotContainder.position.x + itemSize * x, itemSlotContainder.position.y);
            Image image = itemSlotRectTransform.Find("Item").GetComponent<Image>();
            image.sprite = item.GetSprite();
            TextMeshProUGUI uiText = itemSlotRectTransform.Find("amount").GetComponent<TextMeshProUGUI>();

            itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () => {

                inventory.UseItem(item, itemSlotRectTransform.GetSiblingIndex() - 1);
                SetCooldown(item);
                item.isCD = true;
                RefreshInventoryItems();
               

            };

      
            itemSlotRectTransform.GetComponent<Button_UI>().MouseOverFunc = () => {
                Item duplicateItem = new Item { itemType = item.itemType, amount = 1, CD = item.CD , isCD = false};
                inventory.RemoveItem(item, itemSlotRectTransform.GetSiblingIndex() - 1);
                SetCooldown(item);
                ItemWorld.DropItem(character.GetPosition(), duplicateItem, character.GetFacing(), false);
                RefreshInventoryItems();

            };
            
            itemSlotRectTransform.GetComponent<Button_UI>().MouseDownOnceFunc = () => {

                itemSlotRectTransform.GetComponent<Button_UI>().isEnterSlot = true;
            };
            itemSlotRectTransform.GetComponent<Button_UI>().MouseUpFunc = () => {

                itemSlotRectTransform.GetComponent<Button_UI>().isEnterSlot = false;
            };
        




            if (item.amount > 1)
                uiText.SetText(item.amount.ToString());
            else
                uiText.SetText(" ");

            x += 1;




            if (item.isCD)
            {
                SetCooldown(item);
            }
          
        }
    }

    private void SetCooldown(Item item) 
    {
        foreach (Transform child in itemCDContainder)
        {
            for (int i = 0; i < inventory.GetItemList().Count; i++)
            {

                if (item.itemType == inventory.GetItemList()[i].itemType)
                {

                    if (i == child.GetSiblingIndex())
                    {
                       
                        child.gameObject.SetActive(true);
                        child.GetComponent<ItemCooldown>().item = item;
                       
                        child.GetComponent<Button_UI>().MouseOverFunc = () => {
                            Item duplicateItem = new Item { itemType = item.itemType, amount = 1, CD = item.CD, isCD = false };
                            inventory.RemoveItem(item, child.GetSiblingIndex());
                            for (int j = 0; j < inventory.GetItemList().Count; j++)
                            {
                                if (!inventory.GetItemList()[j].isCD)
                                {
                                    itemCDContainder.GetChild(j).gameObject.SetActive(false);
                                }
                            }
                            ItemWorld.DropItem(character.GetPosition(), duplicateItem, character.GetFacing(), false);
                            SetCooldown(item);



                        };
                        child.GetComponent<Button_UI>().MouseDownOnceFunc = () => {

                            child.GetComponent<Button_UI>().isEnterCDSlot = true;
                            isTouched = true;
                        };
                        if (!isTouched)
                        {
                            child.GetComponent<Button_UI>().isEnterCDSlot = false;
                        }

                    }
                    else if (!item.isCD)
                    {
                        child.gameObject.SetActive(false);

                    }
                   
                }

            }
            if(itemCDContainder.childCount > inventory.GetItemList().Count)
            {
                itemCDContainder.GetChild(inventory.GetItemList().Count).gameObject.SetActive(false);

            }
            if (inventory.GetItemList().Count <= 0)
            {
                itemCDContainder.GetChild(0).gameObject.SetActive(false);
            }

           

        }
       
    }

   
}
