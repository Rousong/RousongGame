using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public Animator anim;

    [Header("玩家移动参数")]
    public float speed;
    public float jumpForce;
    public float rollForce;
    public float xVolocity;

    [Header("Ground Check")]
    public Transform groundCheck;// 用来检测地面
    public float checkRadius;
    public LayerMask groundLayer; //图层蒙版

    [Header("States Check")]
    public bool isGround;
    public bool isJump;
    public bool canJump;
    public bool isRoll;
    public bool canRoll;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// 当MonoBehaviour启用时，其Update在每一帧被调用。
    /// </summary>
    private void Update()
    {
        CheckInput();
        xVolocity = rb.velocity.x;
    }
    void CheckInput()
    {
        if (Input.GetAxis("Jump")>0 && isGround)
        {
            canJump = true;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
           // Attack();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            //canRoll = true;

            Roll();
        }
    }

    /// <summary>
    /// 人物移动相关的方法要放在fix update里面
    /// 当MonoBehaviour启用时，其 FixedUpdate在每一帧被调用。
    /// 处理Rigidbody时，需要用FixedUpdate代替Update。例如:给刚体加一个作用力时，
    /// 你必须应用作用力在FixedUpdate里的固定帧，而不是Update中的帧。(两者帧长不同)
    /// </summary>
    private void FixedUpdate()//每秒执行50次
    {
        PhysicsCheck();
        if (!isRoll)
        {
            Movement();
        }
        
        Jump();
    }

    private void PhysicsCheck()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (isGround)
        {
            isJump = false;
           // rb.gravityScale = 1;
        }
    }
    /// <summary>
    /// 可以画出PhysicsCheck()的检测范围
    /// 这个方法是unity自带的一个方法 不需要放到update里面  
    /// </summary>
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }

    void Movement()
    {
        //float horizontalInput = Input.GetAxis("Horizontal");  这样是-1 到1 包含小数
        float horizontalInput = Input.GetAxisRaw("Horizontal");// GetAxisRaw 是整数 一般平台跳跃游戏不需要精准的移动速度

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        // 实现贴图的x轴翻转
        if (horizontalInput != 0)
        {
            transform.localScale = new Vector3(horizontalInput, 1, 1);
        }
    }

    void Jump()
    {
        if (canJump)
        {
            isJump = true;
           // jumpFX.SetActive(true);
            //jumpFX.transform.position = transform.position + new Vector3(0, -0.45f, 0);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
           // rb.gravityScale = 4;
            canJump = false;

        }
    }
 
    void Roll()
    {
            if (rb.velocity.x > 0) 
            {
                isRoll = true;
                anim.Play("roll");
                Debug.Log("duo shan 1");
                rb.velocity = new Vector2(rollForce, 1);
            //rb.AddForce(new Vector2(rollForce, 1));
        }
        else if (rb.velocity.x < 0)
        {
            isRoll = true;
            anim.Play("roll");
            Debug.Log("duo shan 2");
            rb.velocity = new Vector2(-rollForce, 1);
        }     
      }
}
