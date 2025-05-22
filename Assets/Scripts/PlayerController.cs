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
    private float inputX, inputY;

    private bool isGrounded;
    private bool isJump;

    [SerializeField] private Animator animator;
    [SerializeField] private PlayerStatus status;
    [SerializeField] private LayerMask ladderLayer;

    private readonly int IDLE_HASH = Animator.StringToHash("Idle1");
    private readonly int WALK_HASH = Animator.StringToHash("Walk");
    private readonly int JUMP_HASH = Animator.StringToHash("Jump");


    private Vector2 searchY;
    private void OnEnable()
    {
        status.OnJumped += SetJumpAnim;
    }

    private void OnDisable()
    {
        status.OnJumped -= SetJumpAnim;
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
        inputY = Input.GetAxis("Vertical");
    }

    private void PlayerMove()
    {
        if (inputX == 0 && !status.IsJumped)
        {
            animator.Play(IDLE_HASH);
            return;
        }
        if(!status.IsJumped)
        animator.Play(WALK_HASH);

        rigid.velocity = new Vector2(inputX * moveSpeed, rigid.velocity.y);
        spriteRenderer.flipX = inputX < 0 ? true : false;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isGrounded = false;
            isJump = true;
            status.IsJumped = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGrounded) return;
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isJump = false;
            status.IsJumped = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10 && Input.GetKey(KeyCode.W))
        {
            isGrounded = false;
            //animator.오르는 애니메이션
            rigid.velocity = new Vector2(rigid.velocity.x, inputY * moveSpeed);
        }
        if (collision.gameObject.layer == 10 && Input.GetKey(KeyCode.S))
        {
            isGrounded = false;
            //animator.오르는 애니메이션
            rigid.velocity = new Vector2(rigid.velocity.x, inputY * moveSpeed);
        }
    }
    //private RaycastHit2D RayDown()
    //{
    //    Vector2 rayOrigin = transform.position + new Vector3(0, searchY.y);
    //    Debug.DrawRay(rayOrigin, Vector2.down * 1f, Color.red);
    //    RaycastHit2D hitDown = Physics2D.Raycast(rayOrigin, Vector2.down, 1f, ladderLayer);
    //    return hitDown;
    //
    //}
    //
    //private RaycastHit2D RayUp()
    //{
    //    Vector2 rayOrigins = transform.position + new Vector3(0, searchY.y + 3f);
    //    Debug.DrawRay(rayOrigins, Vector2.up * 1f, Color.red);
    //    RaycastHit2D hitUp = Physics2D.Raycast(rayOrigins, Vector2.down, 1f, ladderLayer);
    //    return hitUp;
    //}
    //
    //private void Ladder()
    //{
    //    if (RayDown().collider != null && Input.GetKeyDown(KeyCode.S))
    //    {
    //        rigid.velocity = new Vector2(rigid.velocity.x, inputY * moveSpeed);
    //    }
    //    else if (RayUp().collider != null && Input.GetKeyDown(KeyCode.W))
    //    {
    //        rigid.velocity = new Vector2(rigid.velocity.x, inputY * moveSpeed);
    //    }
    //}

    private void SetJumpAnim(bool value)
    {
        if (value)
            animator.Play(JUMP_HASH);
        else
            animator.Play(IDLE_HASH);
    }
}
