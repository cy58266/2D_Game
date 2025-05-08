using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGuy : Enemy,IDamage
{
    public Transform pickupPoint;
    public float power;
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

    public void PickUpBomb()
    {
        Debug.Log("捡起炸弹");
        if (targetPoint.CompareTag("Bomb") && !hasBomb)
        {
            Debug.Log("捡起炸弹!");
            targetPoint.gameObject.transform.position = pickupPoint.transform.position;
            targetPoint.SetParent(pickupPoint);
            //将重力组件转换
            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            hasBomb = true;
        }
    }

    public void ThrowAway()
    {
        if (hasBomb)
        {
            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            targetPoint.SetParent(transform.parent.parent);

            if (FindObjectOfType<PlayerControll>().gameObject.transform.position.x - transform.position.x < 0)
                targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 1) * power, ForceMode2D.Impulse);
            else
                targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 1) * power, ForceMode2D.Impulse);
        }
        hasBomb = false;
    }

}
