using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInBossBattle : MonoBehaviour
{
   [SerializeField] private Cinemachine.CinemachineVirtualCamera cam;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<character_movement>() != null)
        {
            cam.Follow = null;
            cam.LookAt = null;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<character_movement>() != null)
        {
            cam.Follow = collision.transform;
            cam.LookAt = collision.transform;
        }
    }
}
