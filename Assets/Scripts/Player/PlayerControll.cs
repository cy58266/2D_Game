using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using TMPro;

public class PlayerControll : NetworkBehaviour, IDamage
{
    private Rigidbody2D rb;
    private Animator anim;

    public float speed;
    public float jumpForce;

    [Header("Dash")]
    public float dashSpeed = 25f; // ����ٶ�
    public float dashDuration = 0.2f; // ��̳���ʱ��
    public float dashCooldown = 1f; // �����ȴʱ��

    private bool isDashing = false; // �Ƿ����ڳ��
    private float dashTimeLeft; // ���ʣ��ʱ��
    private float lastDashTime = -Mathf.Infinity; // �ϴγ��ʱ��

    public GameObject afterImagePrefab; // ��ӰԤ����
    public float afterImageInterval = 0.01f; // ��Ӱ���ɼ��

    private float afterImageTimer;


    [Header("Player State")]
    public float health;
    public bool isDead;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;

    [Header("States Check")]
    public bool isGround;
    public bool isJump;
    public bool canJump;

    [Header("Jump FX")]
    public GameObject jumpFX;
    public GameObject landFX;

    [Header("Attack Settings")]
    public GameObject bombPrefab;
    public float nextAttack = 0;
    public float attackRate;

    [Header("Music")]
    public AudioSource audioSource;
    public AudioSource bgmSource;
    public AudioClip hitAudio;
    public AudioClip bgmAudio;

    [Header("Map")]
    public GameObject map;

    [Header("Bag")]
    public GameObject bag;

    [Header("Name")]
    public TMP_Text nameText;

    //��Ҫ��name����ɫͬ����������ң����ͬ�������ı��[SyncVar(hook=nameof(FunctionExecOnClient))]
    [SyncVar(hook = nameof(OnPlayerNameChanged))]
    public string playerName;
    [SyncVar(hook = nameof(OnPlayerColorChanged))]
    private Color playerColor;

    //����OnPlayerNameChanged��OnPlayerColorChanged����������
    //��һ������(oldstr)��ͬ�������޸�ǰ��ֵ���ڶ���(newstr)��ͬ�������޸ĺ��ֵ
    private void OnPlayerNameChanged(string oldstr, string newstr)
    {
        nameText.text = newstr;
    }
    private void OnPlayerColorChanged(Color oldCor, Color newCor)
    {
        nameText.color = newCor;
    }



    //player ��������ƺ���ɫ
    private void ChangedColorAndName()
    {
        //������ƺ���ɫ
        var tempName = $"Player{UnityEngine.Random.Range(1, 999)}";
        var tempColor = new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), 1);

        //ͬ�����������޸�
        CmdSetupPlayer(tempName, tempColor);
    }

    //����ͬ���������޸ģ�ʹ��[Command]���(��Է����ı�ǣ���������Cmd��ͷ)
    //ͨ���������ͬʱ��name����ɫ�����޸�
    [Command]
    private void CmdSetupPlayer(string name, Color color)
    {
        playerName = name;
        playerColor = color;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GameManager.instance.IsPlayer(this);
        health = GameManager.instance.LoadHealth();
        UIManger.instance.UpdateHealth(health);
        audioSource = GetComponent<AudioSource>();
        //bgmSource = GetComponent<AudioSource>();
        hitAudio = Resources.Load<AudioClip>("Hit");
        bgmAudio = Resources.Load<AudioClip>("Bgm");
        bgmSource.Play();
        //GameObject music = GameObject.Find("Audio Source");
        //music.SetActive(false);
        //audioSource.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;


        anim.SetBool("dead", isDead);
        if (isDead)
            return;
        CheckInput();

        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    if (GameObjcetPool.MainInstance == null) {
        //        Debug.Log("123123");
        //        return; }
        //    GameObjcetPool.MainInstance.TakeFromPool();
           
        //    anim.SetTrigger("attack");
        //}


    }

    public void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        if (isDead)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        PhysicsCheck();
        Movement(); 
        Jump();
    }

    void CheckInput()
    {
        if (Input.GetButtonDown("Jump") && isGround)
        {
            isJump = true;
            canJump = true;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            CmdAttack();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            //Debug.Log("!@##@!$@$!@$#");
            if (!map.activeSelf)
                map.SetActive(true);
            else
                map.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!bag.activeSelf)
                bag.SetActive(true);
            else
                bag.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {

            TryDash();
            //Dash();
        }

        if (isDashing)
        {
            Dash();
        }

    }

    public override void OnStartLocalPlayer()
    {

        //��ʼ�����������ɫ������
        ChangedColorAndName();


        rb = GetComponent<Rigidbody2D>(); // ��ȡ�������

        //��������ɫ��
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, Camera.main.transform.position.z);
    }

    void Movement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        if (horizontalInput != 0)
            transform.localScale = new Vector3(horizontalInput, 1, 1);
    }

    void Jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpFX.SetActive(true);
            jumpFX.transform.position = transform.position + new Vector3(0, -0.45f, 0);
            rb.gravityScale = 4;
            canJump = false;
        }
    }


    void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        lastDashTime = Time.time;
    }
    void TryDash()
    {
        // ����Ƿ�����ȴʱ����
        if (Time.time > lastDashTime + dashCooldown)
        {
            StartDash();
        }
    }
    void Dash()
    {
        Debug.Log("���");

        if (dashTimeLeft > 0)
        {
            // ����ƶ�
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");
            Vector2 dashDirection = new Vector2(moveX, moveY).normalized;

            //Debug.Log(moveX + "sda");

            //Debug.Log(dashDirection);

            this.transform.position = new Vector2(this.transform.position.x + dashSpeed * moveX, this.transform.position.y);
            dashTimeLeft -= Time.deltaTime;


            // ���ɲ�Ӱ
            afterImageTimer -= Time.deltaTime;
            if (afterImageTimer <= 0)
            {
                afterImagePrefab.SetActive(true);
                //Instantiate(afterImagePrefab, transform.position, transform.rotation);
                afterImageTimer = afterImageInterval;
                //Destroy(afterImagePrefab);
            }

        }
        else
        {
            // ��̽���
            isDashing = false;
            rb.velocity = Vector2.zero; // ֹͣ�ƶ�
            afterImagePrefab.SetActive(false);
            //Destroy(afterImagePrefab);
        }
    }
    //[ClientRpc]
    [Command]
    public void CmdAttack()
    {
        if (Time.time > nextAttack)
        {
            var obj = Instantiate(bombPrefab, transform.position, bombPrefab.transform.rotation);
            nextAttack = Time.time + attackRate;
            NetworkServer.Spawn(obj);
        }
    }
    public void Attack()
    {
        if (Time.time > nextAttack)
        {
            var obj = Instantiate(bombPrefab, transform.position, bombPrefab.transform.rotation);
            nextAttack = Time.time + attackRate;
            NetworkServer.Spawn(obj);
        }
    }

    //[ClientRpc]
    //public void CreateBomb()
    //{
       
    //}

    void PhysicsCheck()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (isGround)
        {
            rb.gravityScale = 1;
            isJump = false;
        }
        else
        {
            rb.gravityScale = 4;
            isJump = false;
        }
    }

    public void LandFX()//Animation Event
    {
        landFX.SetActive(true);
        landFX.transform.position = transform.position + new Vector3(0, -0.75f, 0);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }

    public void GetHit(float damage)
    {
        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("Player_hit"))
        {
            //Debug.Log("!@#&(!@&$(*&@$!@$");
            //audioSource.gameObject.SetActive(true);
            audioSource.Play();
            bgmSource.Stop();
            health = health - damage;
            if (health < 1)
            {
                health = 0;
                isDead = true;
                audioSource.Stop();
            }
            anim.SetTrigger("hit");

            UIManger.instance.UpdateHealth(health);
        }
    }
}
