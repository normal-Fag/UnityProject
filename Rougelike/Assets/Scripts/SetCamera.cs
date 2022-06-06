using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCamera : MonoBehaviour
{
    [SerializeField] Cinemachine.CinemachineVirtualCamera cinemachine;
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.GetComponent<character_movement>() != null)
        {
            cinemachine.Follow = collision.transform;
            cinemachine.LookAt = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<character_movement>() != null)
        {
            Destroy(gameObject);
        }
    }
}
