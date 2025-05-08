using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyBaseState currentState;

    public Animator anim;
    public int animState;

    private GameObject alarmSign;

    [Header("Base State")]
    public float health;
    public bool isDead;
    public bool hasBomb;
    public bool isBoss;

    [Header("Movement")]
    public float speed;
    public Transform pointA, pointB;
    public Transform targetPoint;

    public PatrolState patrolState = new PatrolState();
    public AttackState attackState = new AttackState();
    public List<Transform> attackList = new List<Transform>();

    [Header("Attack Setting")]
    
    public float attackRate;
    private float nextAttak = 0;
    public float attackRange, skillRange;

    public virtual void Init()
    {
        anim = GetComponent<Animator>();
        alarmSign = transform.GetChild(0).gameObject;
        
    }

    public void Awake()
    {
        Init();
    }
    // Start is called before the first frame update
    void Start()
    {
        TransitionToState(patrolState);
        if (isBoss)
            UIManger.instance.SetBossHealth(health);
        GameManager.instance.IsEnemy(this);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        anim.SetBool("dead", isDead);
        if (isDead)
        {
            UIManger.instance.UpdateBossHealth(0);
            GameManager.instance.EnemyDead(this);
            return;
        }

        currentState.OnUpdate(this);
        anim.SetInteger("state", animState);

        if (isBoss)
            UIManger.instance.UpdateBossHealth(health);
    }

    //×´Ì¬×ª»»
    public void TransitionToState(EnemyBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        FilpDirection();
    }

    public void AttackAction()
    {
        
        if (Vector2.Distance(transform.position, targetPoint.transform.position) < attackRange)
        {
            if (Time.time > nextAttak)
            {
                anim.SetTrigger("attack");
                Debug.Log("ÆÕÍ¨¹¥»÷");
                nextAttak = Time.time + attackRate;
            }
        }
    }

    public virtual void SkillAction()
    {
        
        if (Vector2.Distance(transform.position, targetPoint.transform.position) < skillRange)
        {
            if (Time.time > nextAttak)
            {
                anim.SetTrigger("skill");
                Debug.Log("¼¼ÄÜ¹¥»÷");
                nextAttak = Time.time + attackRate;
            }
        }
    }

    public void FilpDirection()
    {
        if (transform.position.x < targetPoint.position.x)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    public void SwitchPoint()
    {
        if (Mathf.Abs(pointA.position.x - transform.position.x) > Mathf.Abs(pointB.position.x - transform.position.x))
        {
            targetPoint = pointA;
        }
        else
        {
            targetPoint = pointB;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        
        if (!attackList.Contains(collision.transform) && !hasBomb && !isDead&&!GameManager.instance.gameOver && LayerMask.LayerToName(collision.gameObject.layer) != "Trap")
            attackList.Add(collision.transform);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        attackList.Remove(collision.transform);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isDead && !GameManager.instance.gameOver)
            StartCoroutine(OnAlarm());
    }

    IEnumerator OnAlarm()
    {
        alarmSign.SetActive(true);
        yield return new WaitForSeconds(alarmSign.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        alarmSign.SetActive(false);
    }

}
