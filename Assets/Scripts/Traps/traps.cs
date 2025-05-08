using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class traps : MonoBehaviour
{

    public float trapsFroce = 20f;

    public bool canMove = false;

    public float moveSpeed = 5f; // �ƶ��ٶ�
    public float rotationSpeed = 200f; // ��ת�ٶ�
    public float raycastDistance = 1f; // ���߼�����

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
            MoveForward();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && LayerMask.LayerToName(other.gameObject.layer) == "Player")
        {
            //Debug.Log("Player Get Hut From Trap");

            Vector3 pos = transform.position - other.transform.position;
            //Debug.Log("POS:" + pos);
            other.GetComponent<Rigidbody2D>().AddForce((-pos + Vector3.up) * trapsFroce, ForceMode2D.Impulse);

            other.GetComponent<IDamage>().GetHit(1);
        }
    }

    void MoveForward()
    {
        // ��ǰ�ƶ�
        CheckForWall();
        transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
    }

    void CheckForWall()
    {
        // �������߼��ǰ���Ƿ���ǽ
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up * .5f, transform.up, raycastDistance, LayerMask.GetMask("Ground"));


        if (hit.collider != null)
        {
            //print(hit.collider.name);
            // �����⵽ǽ����������
            if (hit.collider.CompareTag("Wall"))
            {

                RotateAlongWall(hit.normal);
            }
        }

        //Debug.DrawRay(transform.position, transform.up * raycastDistance, Color.red);
    }

    void RotateAlongWall(Vector2 wallNormal)
    {
        // �����µķ�������ǽ�ķ���
        Vector2 newDirection = Vector2.Perpendicular(wallNormal).normalized;

        // ƽ����ת���·���
        float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        //Quaternion targetRotation = Quaternion.Euler(0, 0, 90);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position + Vector3.up * .5f, transform.up * raycastDistance, Color.red);
    }

}
