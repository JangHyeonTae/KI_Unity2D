using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{

    [Header("UI")]
    [SerializeField] private HpGuage hpGuage;

    private int hp;
    public int Hp { get { return hp; } set { hp = value; } }
    private int maxHp = 100;
    public int MaxHp { get { return maxHp; } set { maxHp = value; } }
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float attackRange;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private float inputX, inputY;

    private bool isGrounded;
    private bool isJump;
    private bool isAttack;

    [SerializeField] private Animator animator;
    //[SerializeField] private PlayerStatus status;
    [SerializeField] private LayerMask ladderLayer;
    [SerializeField] private LayerMask enemyLayer;

    private readonly int IDLE_HASH = Animator.StringToHash("Idle1");
    private readonly int WALK_HASH = Animator.StringToHash("Walk");
    private readonly int JUMP_HASH = Animator.StringToHash("Jump");
    private readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    private GameObject target;
    private Vector2 searchY;

    Coroutine attackCor;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Hp = MaxHp;
    }

    void Update()
    {
        PlayerInput();
        AttackAnim();
    }

    private void FixedUpdate()
    {
        PlayerMove();
        if (isGrounded)
        {
            Jump();
        }
    }


    private IEnumerator CorAttack()
    {
        isAttack = true;
        animator.Play(ATTACK_HASH);
        yield return new WaitForSeconds(0.7f);

        isAttack = false;
        attackCor = null;
    }

    private void PlayerInput()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
    }

    private void PlayerMove()
    {
        if (!isAttack)
        { 
            if(inputX == 0)
            {
                animator.Play(IDLE_HASH);
                return;
            }

            animator.Play(WALK_HASH);

            rigid.velocity = new Vector2(inputX * moveSpeed, rigid.velocity.y);
            spriteRenderer.flipX = inputX < 0 ? true : false;
        }
        
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isGrounded = false;
            isJump = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGrounded) return;
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isJump = false;
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


    private void AttackAnim()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (attackCor == null)
            {
                attackCor = StartCoroutine(CorAttack());
            }
        }
    }

    public void Attack()
    {
        IDamagable setTarget = AttackSearch();
        if (setTarget == null) return;
        setTarget.TakeDamage(10);
    }

    private IDamagable AttackSearch()
    {
        Vector2 pos = transform.position + new Vector3(spriteRenderer.flipX ? -1f : 1f, 0);
        Collider2D hit = Physics2D.OverlapCircle(pos, attackRange, enemyLayer);
        
        if (hit == null) return null;
        Debug.Log("Hit");
        return hit.GetComponent<IDamagable>();
    }


    public void TakeDamage(int amount)
    {
        Hp = Mathf.Max(0, Hp - amount);
        SetHpGuage();
    }

    public void Heal(int amount)
    {
        Hp = Mathf.Min(MaxHp, Hp + amount);
        SetHpGuage();
    }

    public void SetHpGuage()
    {
        float percent = Hp / (float)MaxHp;
        hpGuage.GetGuage(percent);
    }

}
