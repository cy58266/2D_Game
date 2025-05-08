using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : Enemy,IDamage
{
    //public Transform pickupPoint;
    //public float power;
    public void GetHit(float damage)
    {
        health = health - damage;
        if (health < 1)
        {
            health = 0;
            isDead = true;
            anim.SetBool("dead", true);
            //Debug.Log("╪дак");
            //GetComponentInChildren<HitPoint>().isDead = true;
        }
        anim.SetTrigger("hit");
    }

    public void OnDestroy()
    {
        //Debug.Log("╪дак!");
        this.gameObject.SetActive(false);
    }

}
