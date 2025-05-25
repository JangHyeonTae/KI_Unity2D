using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    //입력이 들어올공간
    public StateMachine stateMachine;

    [Header("UI")]
    public HpGuage hpGuage;

    private int hp;
    public int Hp { get { return hp; } set { hp = value; } }
    
    private int maxHp = 100;
    public int MaxHp { get { return maxHp; } set { maxHp = value; } }
    
    public float moveSpeed;
    public float upSpeed;
    public float jumpPower;
    public float attackRange;

    public Collider2D playerCol;

    public Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer;
    public float inputX, inputY;

    public bool isGrounded;
    public bool isJumped;
    public bool canAttacked;
    public bool isAttacked;
    public bool isLaddered;
    private bool isTrig;
    public bool IsTrig { get { return isTrig; } set { isTrig = value;  OnTrig?.Invoke(isTrig); } }
    public event Action<bool> OnTrig;

    [SerializeField] private LayerMask ladderLayer;
    [SerializeField] private LayerMask enemyLayer;

    public Animator animator;

    public readonly int IDLE_HASH = Animator.StringToHash("Idle1");
    public readonly int WALK_HASH = Animator.StringToHash("Walk");
    public readonly int JUMP_HASH = Animator.StringToHash("Jump");
    public readonly int ATTACK_HASH = Animator.StringToHash("Attack");
    public readonly int LADDER_HASH = Animator.StringToHash("Ladder");

    public Coroutine attackCor;

    private void OnEnable()
    {
        OnTrig += SetTrig;
    }

    private void OnDisable()
    {
        OnTrig -= SetTrig;
    }

    private void SetTrig(bool value)
    {
        isTrig = value;
        playerCol.isTrigger = isTrig;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StateMachineInit();
    }

    private void StateMachineInit()
    {
        stateMachine = new StateMachine();
        stateMachine.stateDic.Add(EState.Idle, new Player_Idle(this));
        stateMachine.stateDic.Add(EState.Walk, new Player_Walk(this));
        stateMachine.stateDic.Add(EState.Jump, new Player_Jump(this));
        stateMachine.stateDic.Add(EState.Attack, new Player_Attack(this));
        stateMachine.stateDic.Add(EState.Ladder, new Player_Ladder(this));
        stateMachine.CurState = stateMachine.stateDic[EState.Idle];
    }

    private void Update()
    {
        //입력
        inputX = Input.GetAxis("Horizontal");
        isJumped = Input.GetKeyDown(KeyCode.Space);
        canAttacked = Input.GetKeyDown(KeyCode.Q);
        inputY = Input.GetAxisRaw("Vertical");

        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            Debug.Log("true");
            isLaddered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            Debug.Log("false");
            isLaddered = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            isGrounded = true;
        }

    }


    public void AttackAnim()
    {
         if (attackCor == null)
         {
             attackCor = StartCoroutine(CorAttack());
         }
    }

    private IEnumerator CorAttack()
    {
        yield return new WaitForSeconds(0.7f);
        attackCor = null;
        isAttacked = false;
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
    }

    public void SetHpGuage()
    {
        float percent = Hp / (float)MaxHp;
        hpGuage.GetGuage(percent);
    }
}
