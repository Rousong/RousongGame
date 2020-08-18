using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 
/// </summary>
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public Animator anim;

    [Header("依赖脚本")]
    [SerializeField] CharacterAudio audioEffectPlayer = null;


    [Header("玩家移动参数")]
    float horizontalInput;
    public bool canMove = true; // 玩家是否可以移动
    public bool airControl = false; // 玩家在跳跃时是否可以转向
    public float speed;
    public float jumpForce;
    public float rollForce;
    public float xVolocity; //test ===================================
    public float yVolocity; //test ===================================
    public float playerGravity; //test ===================================

    [SerializeField] private float dashForce = 25f;
    private float limitFallSpeed = 25f; // Limit fall speed


    [Header("物理碰撞检测")]
    public Transform groundCheck; // 用来检测地面
    public Transform wallCheck; // 用来检测墙壁

    public float checkRadius = 1f; // 检测的范围
    public LayerMask groundLayer; // 地面图层蒙版

    [Header("状态检测")]
    public bool isOnGround;
    public bool isWall;
    private bool facingRight = true;  // For determining which way the player is currently facing.

    [Header("跳跃相关")]
    public bool isJumping;
    public bool canJump;
    public bool canDoubleJump = true; //If player can double jump

    [Header("冲刺相关")]
    private bool dash;
    public bool isDashing;
    public bool canDash = true;

    [Header("贴墙跳相关")]
    public bool isWallSliding;
    public bool oldWallSlidding;
    public float prevVelocityX = 0f;
    public bool canCheck = false; //For check if player is wallsliding
    public float jumpWallStartX = 0;
    public float jumpWallDistX = 0; //Distance between player and wall
    public bool limitVelOnWallJump = false; //For limit wall jump distance with low fps


    [Header("事件")]
    [Space]

    public UnityEvent OnFallEvent;
    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (OnFallEvent == null)
            OnFallEvent = new UnityEvent();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
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
        xVolocity = rb.velocity.x; //test ------------------------------------------------------------
        yVolocity = rb.velocity.y; //test ------------------------------------------------------------
        playerGravity = rb.gravityScale; //test ------------------------------------------------------------
    }
    void CheckInput()
    {
        //float horizontalInput = Input.GetAxis("Horizontal");  这样是-1 到1 包含小数
         horizontalInput = Input.GetAxisRaw("Horizontal");// GetAxisRaw 是整数 一般平台跳跃游戏不需要精准的移动速度

        if (Input.GetKeyDown(KeyCode.Space))
        {
            canJump = true;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
           // Attack();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            dash = true;
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
        Movement(horizontalInput,dash);
        Jump();
        dash = false;
        canJump = false;
    }
    /// <summary>
    /// 检测玩家的物理碰撞
    /// </summary>
    private void PhysicsCheck()
    {
        bool wasGrounded = isOnGround;
        isOnGround = false;

        // 检测玩家是否在地面上
        /*
         isOnGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
         if (isOnGround)
         {
             isJumping = false;
             rb.gravityScale = 1;
         }
        */

        // 检测玩家是否在地面上
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, checkRadius, groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            // 如果玩家在地面上
            if (colliders[i].gameObject != gameObject)
                isOnGround = true;
                isJumping = false;
                rb.gravityScale = 1; //重力设为1
            if (!wasGrounded)
            {
                OnLandEvent.Invoke();
                if (!isWall && !isDashing) //如果玩家没有在墙上 也没有在冲刺状态
                    // particleJumpDown.Play();
                canDoubleJump = true; //可以二段跳
                if (rb.velocity.y < 0f)
                    limitVelOnWallJump = false; //如果下落的加速度是0， 那么玩家不可以贴墙跳
            }
        }
    
        // 检测玩家是否贴墙
        isWall = false;
        if (!isOnGround)
        {
            OnFallEvent.Invoke();
            Collider2D[] collidersWall = Physics2D.OverlapCircleAll(wallCheck.position, checkRadius, groundLayer);
            for (int i = 0; i < collidersWall.Length; i++)
            {
                if (collidersWall[i].gameObject != null)
                {
                    isDashing = false;
                    isWall = true;
                }
            }
            prevVelocityX = rb.velocity.x;
        }
        if (limitVelOnWallJump)
        {
            if (rb.velocity.y < -0.5f)
                limitVelOnWallJump = false;
            jumpWallDistX = (jumpWallStartX - transform.position.x) * transform.localScale.x;
            if (jumpWallDistX < -0.5f && jumpWallDistX > -1f)
            {
                canMove = true;
            }
            else if (jumpWallDistX < -1f && jumpWallDistX >= -2f)
            {
                canMove = true;
                rb.velocity = new Vector2(10f * transform.localScale.x, rb.velocity.y);
            }
            else if (jumpWallDistX < -2f)
            {
                limitVelOnWallJump = false;
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else if (jumpWallDistX > 0)
            {
                limitVelOnWallJump = false;
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
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
        Gizmos.DrawWireSphere(wallCheck.position, checkRadius);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Movement(float horizontalInput, bool dash)
    {
        if (canMove)
        {
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

            // 实现贴图的x轴翻转
            if (horizontalInput != 0)
            {
                transform.localScale = new Vector3(horizontalInput, 1, 1);
            }
            if (dash && canDash && !isWallSliding)
            {
                //m_Rigidbody2D.AddForce(new Vector2(transform.localScale.x * m_DashForce, 0f));
                StartCoroutine(DashCooldown());
            }
            if (isDashing)
            {
                rb.velocity = new Vector2(transform.localScale.x * dashForce, 0);
                audioEffectPlayer.Play(CharacterAudio.AudioType.Dash, true);
            }

        }
    }

    void Jump()
    {
        // 如果玩家在地面上and可以跳起的状态
        if (isOnGround &&  canJump  )
        {
            isJumping = true;
            // jumpFX.SetActive(true);
            //jumpFX.transform.position = transform.position + new Vector3(0, -0.45f, 0);
            isOnGround = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            rb.gravityScale = 4;
            // canJump = false;

            // AudioManager.instance.PlayJumpAudio();
            audioEffectPlayer.Play(CharacterAudio.AudioType.Jump, true);
        }
        // 如果玩家处于不在地面但是可以跳起而且可以2段跳，并且不在贴墙的状态下
        else if (!isOnGround && canJump && canDoubleJump && !isWallSliding)
        {
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0f, jumpForce*50));
            audioEffectPlayer.Play(CharacterAudio.AudioType.Jump, true);
        }
    }
 
    void Roll()
    {
            if (rb.velocity.x > 0) 
            {
                isDashing = true;
                anim.Play("roll");
                Debug.Log("duo shan 1");
                rb.velocity = new Vector2(rollForce, 1);
            //rb.AddForce(new Vector2(rollForce, 1));
        }
        else if (rb.velocity.x < 0)
        {
            isDashing = true;
            anim.Play("roll");
            Debug.Log("duo shan 2");
            rb.velocity = new Vector2(-rollForce, 1);
        }     
      }

    IEnumerator DashCooldown()
    {
        anim.SetBool("dash", true);
        isDashing = true;
        canDash = false;
        yield return new WaitForSeconds(0.1f);
        isDashing = false;
        yield return new WaitForSeconds(0.5f);
        canDash = true;
    }
}
