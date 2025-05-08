using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whale : Enemy,IDamage
{
    public float scale;
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

    public void Swalow()
    {
        targetPoint.GetComponent<Bomb>().TurnOff();
        targetPoint.gameObject.SetActive(false);

        transform.localScale *= scale;
    }

}
