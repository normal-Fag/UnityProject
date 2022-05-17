using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemyBehavior : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!staticEnemy && inRange && target.GetComponent<playerMovement>().isLiving)
            EnemyTrigger();
    }

    public override void EnemyTrigger()
    {
        
    }

    override public void Move()
    {
        rb.MovePosition(target.position);
    }
}
