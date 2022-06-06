using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosionPoolScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<character_movement>() != null)
        {
            collision.GetComponent<character_movement>().isPosioned = true;
            collision.GetComponent<character_movement>().isExitPosionPool = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<character_movement>() != null)
        {
            collision.GetComponent<character_movement>().isPosioned = false;
            collision.GetComponent<character_movement>().isExitPosionPool = true;
        }
    }
}
