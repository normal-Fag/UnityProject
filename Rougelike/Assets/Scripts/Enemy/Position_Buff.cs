using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position_Buff : MonoBehaviour
{
    private int cacheChild;
    void Start()
    {
        cacheChild = transform.childCount;
    }

    void Update()
    {
        if(cacheChild != transform.childCount)
        {
            int i = 0;

            foreach(Transform child in transform)
            {

                child.position = new Vector2(transform.position.x + (transform.parent.Find("Slot_2").position.x - transform.parent.Find("Slot_1").position.x) / 1.7f * i, transform.position.y);
                i++;
            }
        }
        else
        {
            cacheChild = transform.childCount;
        }
    }
}
