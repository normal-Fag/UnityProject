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
    public Transform buffEffectContainder;
    public Transform buffEffectItem;



    public Transform itemDescrtptionContainer;
    public Transform itemDescrtptionSlot;

    public Transform popupChoise;

    private character_movement character;

    private bool isTouched;

    private void Awake()
    {
        itemSlotContainder = transform.Find("ItemSlotContainer");
        itemSlot = itemSlotContainder.Find("ItemSlot");
        itemCDContainder = transform.Find("CDSlotContainer"); 
        itemCD = itemCDContainder.Find("CD");
        buffEffectContainder = transform.Find("BuffEffectContainer");
        buffEffectItem = buffEffectContainder.Find("BuffEffectItem");
        itemDescrtptionContainer = transform.Find("PopUpDescriptionContainer");
        itemDescrtptionSlot = itemDescrtptionContainer.Find("ItemForSlot");
        popupChoise = transform.Find("PopUpChoise");
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

        float itemSize = transform.Find("Slot_2").position.x  - transform.Find("Slot_1").position.x;

        foreach (Item item in inventory.GetItemList())
        {
            isTouched = false;
            Transform itemSlotRectTransform = Instantiate(itemSlot, itemSlotContainder).GetComponent<Transform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.position = new Vector2(itemSlot.position.x + itemSize * x, itemSlotContainder.position.y);
            Image image = itemSlotRectTransform.Find("Item").GetComponent<Image>();
            image.sprite = item.GetSprite();
            TextMeshProUGUI uiText = itemSlotRectTransform.Find("amount").GetComponent<TextMeshProUGUI>();

          
   
            itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () => {

                inventory.UseItem(item, itemSlotRectTransform.GetSiblingIndex() - 1);
                SetBuffCD(item);
                SetCooldown(item);
                item.isCD = true;
                popupChoise.gameObject.SetActive(false);
                if (itemDescrtptionContainer.childCount > 1)
                {
                    bool isSecond = false;
                    foreach (Transform child in itemDescrtptionContainer)
                    {
                        if (isSecond)
                        {
                            Destroy(child.gameObject);
                        }
                        else
                        {
                            isSecond = true;
                        }
                    }
                }
                RefreshInventoryItems();
               

            };

            itemSlotRectTransform.GetComponent<Button_UI>().MouseOverFunc = () => {

                if (itemDescrtptionContainer.childCount > 1)
                {
                    bool isSecond = false;
                    foreach (Transform child in itemDescrtptionContainer)
                    {
                        if (isSecond)
                        {
                            Destroy(child.gameObject);
                        }
                        else
                        {
                            isSecond = true;
                        }
                    }
                }

                popupChoise.gameObject.SetActive(true);
                popupChoise.position = new Vector2(itemSlotRectTransform.position.x, itemSlotRectTransform.position.y + itemSize);
                popupChoise.Find("drop").GetComponent<Button_UI>().MouseDownOnceFunc = () =>
                {
                    Item duplicateItem = new Item { itemType = item.itemType, amount = 1, isCD = false};
                    inventory.RemoveItem(item, itemSlotRectTransform.GetSiblingIndex() - 1);
                    SetCooldown(item);
                    ItemWorld.DropItem(character.GetPosition(), duplicateItem, character.GetFacing(), false);
                    popupChoise.gameObject.SetActive(false);
                    RefreshInventoryItems();
                };


                popupChoise.Find("info").GetComponent<Button_UI>().MouseDownOnceFunc = () =>
                {
                    
                    popupChoise.gameObject.SetActive(false);
                    Transform itemInfoRectTransform = Instantiate(itemDescrtptionSlot, itemDescrtptionContainer).GetComponent<Transform>();
                    itemInfoRectTransform.gameObject.SetActive(true);
                    itemInfoRectTransform.position = new Vector2(itemSlotRectTransform.position.x, itemSlotRectTransform.position.y + itemSize);
                    itemInfoRectTransform.GetComponent<Popup_items>().item = item;

                };
               

            };


            itemSlotRectTransform.GetComponent<Button_UI>().MouseDownOnceFunc = () => {
                popupChoise.gameObject.SetActive(false);
                if (itemDescrtptionContainer.childCount > 1)
                {
                    bool isSecond = false;
                    foreach (Transform child in itemDescrtptionContainer)
                    {
                        if (isSecond)
                        {
                            Destroy(child.gameObject);
                        }
                        else
                        {
                            isSecond = true;
                        }
                    }
                }
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

    public void SetBuffCD(Item item)
    {

        if (item.IsInfinityBuff())
        {
            bool hasInScreen = false;
            foreach (Transform child in buffEffectContainder)
            {
                if (child.GetComponent<BuffEffect_UI>().item != null && child.GetComponent<BuffEffect_UI>().item.itemType == item.itemType && !item.IsMajorBuff())
                {
                    child.GetComponent<BuffEffect_UI>().countBuff += 1;
                    hasInScreen = true;
                }
                if(child.GetComponent<BuffEffect_UI>().item != null && child.GetComponent<BuffEffect_UI>().item.IsMajorBuff() && item.IsMajorBuff())
                {
                    Destroy(child.gameObject);
                }
            }
            if (!hasInScreen)
            {
                Transform buffEffectslotRectTransform = Instantiate(buffEffectItem, buffEffectContainder).GetComponent<Transform>();
                Destroy(buffEffectslotRectTransform.Find("GreenBackground").gameObject);
                Destroy(buffEffectslotRectTransform.Find("timer").gameObject);
                buffEffectslotRectTransform.gameObject.SetActive(true);
                buffEffectslotRectTransform.GetComponent<BuffEffect_UI>().item = item;
                Image image_buff = buffEffectslotRectTransform.Find("item").GetComponent<Image>();
                image_buff.sprite = item.GetSprite();
            }
        }
      
       if (item.itemType != Item.ItemType.HealthPotion && item.itemType != Item.ItemType.ManaPotion && !item.IsInfinityBuff())
        {
            Transform buffEffectslotRectTransform = Instantiate(buffEffectItem, buffEffectContainder).GetComponent<Transform>();
            buffEffectslotRectTransform.gameObject.SetActive(true);
            buffEffectslotRectTransform.GetComponent<BuffEffect_UI>().item = item;
            Image image_buff = buffEffectslotRectTransform.Find("item").GetComponent<Image>();
            image_buff.sprite = item.GetSprite();
        }

    }

    private void SetCooldown(Item item) 
    {

        float itemSize = transform.Find("Slot_2").position.x - transform.Find("Slot_1").position.x;
        foreach (Transform child in itemCDContainder)
        {
            for (int i = 0; i < inventory.GetItemList().Count; i++)
            {

                if (item.itemType == inventory.GetItemList()[i].itemType)
                {

                    if (i == child.GetSiblingIndex() && !item.IsInfinityBuff())
                    {
                       
                        child.gameObject.SetActive(true);
                        child.GetComponent<ItemCooldown>().item = item;
                       
                        child.GetComponent<Button_UI>().MouseOverFunc = () => {

                            if (itemDescrtptionContainer.childCount > 1)
                            {
                                bool isSecond = false;
                                foreach (Transform child in itemDescrtptionContainer)
                                {
                                    if (isSecond)
                                    {
                                        Destroy(child.gameObject);
                                    }
                                    else
                                    {
                                        isSecond = true;
                                    }
                                }
                            }
                            isTouched = false;
                            popupChoise.gameObject.SetActive(true);
                            popupChoise.position = new Vector2(child.position.x, child.position.y + itemSize);
                            popupChoise.Find("drop").GetComponent<Button_UI>().MouseDownOnceFunc = () =>
                            {
                                Item duplicateItem = new Item { itemType = item.itemType, amount = 1, isCD = false };
                                inventory.RemoveItem(item, child.GetSiblingIndex());
                                for (int j = 0; j < inventory.GetItemList().Count; j++)
                                {
                                    if (!inventory.GetItemList()[j].isCD)
                                    {
                                        itemCDContainder.GetChild(j).gameObject.SetActive(false);
                                    }
                                }
                                ItemWorld.DropItem(character.GetPosition(), duplicateItem, character.GetFacing(), false);
                                popupChoise.gameObject.SetActive(false);
                                SetCooldown(item);
                      
                            };

                            popupChoise.Find("info").GetComponent<Button_UI>().MouseDownOnceFunc = () =>
                            {

                                popupChoise.gameObject.SetActive(false);
                                Transform itemInfoRectTransform = Instantiate(itemDescrtptionSlot, itemDescrtptionContainer).GetComponent<Transform>();
                                itemInfoRectTransform.gameObject.SetActive(true);
                                itemInfoRectTransform.position = new Vector2(child.position.x, child.position.y + itemSize);
                                itemInfoRectTransform.GetComponent<Popup_items>().item = item;

                            };



                        };
                        child.GetComponent<Button_UI>().MouseDownOnceFunc = () => {
                            popupChoise.gameObject.SetActive(false);
                            if (itemDescrtptionContainer.childCount > 1)
                            {
                                bool isSecond = false;
                                foreach (Transform child in itemDescrtptionContainer)
                                {
                                    if (isSecond)
                                    {
                                        Destroy(child.gameObject);
                                    }
                                    else
                                    {
                                        isSecond = true;
                                    }
                                }
                            }

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
