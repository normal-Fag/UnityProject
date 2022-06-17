using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapTrack : MonoBehaviour
{
    public GameObject swapTrigger;
    public AudioClip newTrack;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<character_movement>() != null)
        {
            WorldAudioManager.instance.SwapTrack(newTrack);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.GetComponent<character_movement>() != null)
        {
            swapTrigger.SetActive(true);
            gameObject.SetActive(false);
        }
      
    }
}
