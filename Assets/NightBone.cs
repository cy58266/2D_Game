using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightBone : Enemy, IDamage
{
    public void GetHit(float damage)
    {
        health = health - damage;
        if (health < 1)
        {
            health = 0;
            isDead = true;
            anim.SetBool("dead", true);
            //this.gameObject.SetActive(false);
            //GetComponentInChildren<HitPoint>().isDead = true;
        }
        anim.SetTrigger("hit");
    }

    public void OnDestroy()
    {
        this.gameObject.SetActive(false);
    }
}
