using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJumping : MonoBehaviour
{
    private Rigidbody2D rb;
    public float fallMultiplier = 6.5f;
    public float lowJumpMultiplier = 6f;
    public bool isJumpPressed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        isJumpPressed = Input.GetButton("Jump");
  
        if (rb.velocity.y < 0)// 当玩家下落的时候
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }else if(rb.velocity.y > 0 && !Input.GetButton("Jump")) // 当玩家上升 并且没有按住跳跃键的时候
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
