using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit_test : MonoBehaviour
{
    private Animator m_animator;

    public int max_hp = 100;
    int currentHp;
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        currentHp = max_hp;
    }


    public void Take_Damage(int damage)
    {
        currentHp -= damage;
        m_animator.SetTrigger("Hurt");

        if (currentHp <= 0)
        {
            m_animator.SetTrigger("Death");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
