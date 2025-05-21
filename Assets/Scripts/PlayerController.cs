using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private float inputX;

    private bool isGrounded;
    private bool isJump;

    [SerializeField] private Animator animator;

    [SerializeField] private PlayerStatus status;

    private readonly int IDLE_HASH = Animator.StringToHash("Idle1");
    private readonly int WALK_HASH = Animator.StringToHash("Walk");
    private readonly int JUMP_HASH = Animator.StringToHash("Jump");


    private void OnEnable()
    {
        status.OnJumped += SetJumpAnim;
        status.OnWalk += SetWalkAnim;
    }

    private void OnDisable()
    {
        status.OnJumped -= SetJumpAnim;
        status.OnWalk -= SetWalkAnim;
    }
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        PlayerMove();
        if (isGrounded)
        {
            Jump();
        }
    }

    private void PlayerInput()
    {
        inputX = Input.GetAxis("Horizontal");
    }

    private void PlayerMove()
    {
        if (inputX == 0)
        {
            animator.Play(IDLE_HASH);
            //status.IsWalk = false;
            return;
        }


        //status.IsWalk = true;
        animator.Play(WALK_HASH);
        rigid.velocity = new Vector2(inputX * moveSpeed, rigid.velocity.y);
        //if (inputX < 0)
        //{
        //    spriteRenderer.flipX = true;
        //}
        //else
        //{
        //    spriteRenderer.flipX = false;
        //}
        spriteRenderer.flipX  = inputX < 0 ? true : false;
        
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isGrounded = false;
            animator.Play(JUMP_HASH);
            isJump = true;
            //status.IsJumped = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isJump = false;
            //status.IsJumped = false;
        }
    }


    private void SetJumpAnim(bool value) => animator.SetBool("IsJump", value);
    private void SetWalkAnim(bool value) => animator.SetBool("IsWalk", value);
}
