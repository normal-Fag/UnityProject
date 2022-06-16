using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCamera : MonoBehaviour
{
    [SerializeField] Cinemachine.CinemachineVirtualCamera cinemachine;
    [SerializeField] GameObject point;
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.GetComponent<character_movement>() != null)
        {
            cinemachine.Follow = collision.transform;
            cinemachine.LookAt = collision.transform;
            point.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.NearClipPlane = 0;
            point.GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 1;
            collision.transform.Find("Audio Manager").gameObject.SetActive(true);
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
