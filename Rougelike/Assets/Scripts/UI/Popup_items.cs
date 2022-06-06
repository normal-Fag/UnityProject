using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Popup_items : MonoBehaviour
{
    private Text nameItem;
    private Text descriptionItem;
    public Item item;
    public bool isActive = false;
    void Start()
    {
        nameItem = transform.Find("ItemName").GetComponent<Text>();
        descriptionItem = transform.Find("Description").GetComponent<Text>();
        nameItem.text = item.GetName();
        descriptionItem.text = item.GetDescription();

    }

    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Destroy"))
        {
            Destroy(gameObject);
        }
      
    }
}
