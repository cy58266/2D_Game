using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaldPriate : Enemy, IDamage
{
    
    public void GetHit(float damage)
    {
        health = health - damage;
        if (health < 1)
        {
            health = 0;
            isDead = true;
            GetComponentInChildren<HitPoint>().isDead = true;
        }
        anim.SetTrigger("hit");
    }

    

}
