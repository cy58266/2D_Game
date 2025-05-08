using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightBoneHitPoint : MonoBehaviour
{
    public bool bombAvilable;
    public bool isDead;
    int dir;

    public Animator nit;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (transform.position.x > other.transform.position.x)
            dir = -1;
        else
            dir = 1;

        if (other.CompareTag("Player") && !isDead)
        {
            Debug.Log("Player Get Hut");
            other.GetComponent<IDamage>().GetHit(1);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 1) * 10, ForceMode2D.Impulse);
        }

        if (other.CompareTag("Bomb") && bombAvilable && !isDead)
        {
            Debug.Log("Orc Find Bomb!");
            nit.SetTrigger("skill");
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 0) * 10, ForceMode2D.Impulse);
        }
    }
}
