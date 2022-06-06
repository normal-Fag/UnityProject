using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingZone : MonoBehaviour
{

    [SerializeField] private Cinemachine.CinemachineVirtualCamera Cinemachine;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<character_movement>() != null)
        {
            Cinemachine.Follow = null;
            StartCoroutine(collision.GetComponent<character_movement>().ResetLevel());
        }
    }
}
