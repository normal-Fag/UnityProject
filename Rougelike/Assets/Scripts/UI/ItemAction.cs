using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ItemAction : MonoBehaviour
{
    private Transform ui_inventory;
    public Item item;
    public float timer = 0;
    public int index;
    private character_movement character;
    private bool isSecondClick = false;
    private float timerForClick;
    void Start()
    {
        ui_inventory = transform.parent.parent;
    }

    void Update() { }
    
}
