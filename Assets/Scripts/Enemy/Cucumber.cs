using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : Enemy, IDamage
{
    //Animation Event
    public void SetOff()
    {
        if(targetPoint)
            targetPoint.GetComponent<Bomb>().TurnOff();
    }

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
