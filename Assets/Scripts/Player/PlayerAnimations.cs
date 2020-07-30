using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class PlayerAnimations : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerController controller;
    private CharacterData player;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
        player = GetComponent<CharacterData>();
    }

    private void Update()
    {
        anim.SetFloat("speed", math.abs(rb.velocity.x));
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("jump", controller.isJump);
        anim.SetBool("ground", controller.isGround);
        anim.SetBool("roll", controller.isRoll);
        anim.SetInteger("state", player.state);
    }

    void stopRolling()
    {
        controller.isRoll = false;
    }
}
