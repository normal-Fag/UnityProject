using UnityEngine;
using System.Collections;

public class Sensor_Prototype : MonoBehaviour {

    private int m_ColCount = 0;

    private float m_DisableTimer;

    private void OnEnable()
    {
        m_ColCount = 0;
    }

    public bool State()
    {
        if (m_DisableTimer > 0)
            return false;
        return m_ColCount > 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Ground")
        {
            m_ColCount++;
        }
 

        if(other.GetComponent<Enemy>() != null && transform.parent.GetComponent<character_movement>() != null)
        {
            transform.parent.GetComponent<character_movement>().GetComponent<Collider2D>().isTrigger = true;
        }
        else if(transform.parent.GetComponent<character_movement>() != null)
        {
            transform.parent.GetComponent<character_movement>().GetComponent<Collider2D>().isTrigger = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            m_ColCount--;
        }
    }

    void Update()
    {
        m_DisableTimer -= Time.deltaTime;
    }

    public void Disable(float duration)
    {
        m_DisableTimer = duration;
    }
}
