using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")&& LayerMask.LayerToName(other.gameObject.layer) == "Player")
        {

            //Debug.Log("Player Get Heart");

            //other.GetComponent<IDamage>().GetHit(1);
            other.GetComponent<IDamage>().GetHit(-1);

            this.gameObject.SetActive(false);
            //Destroy(this.gameObject);

            //other.gameObject.GetComponent
        }
    }
}
