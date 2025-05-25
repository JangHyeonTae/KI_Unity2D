using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Monsters : MonoBehaviour, IDamagable
{
    public int hp;
    public int maxHp = 100;

    public StateMachine stateMachine;

    public float moveSpeed = 5f;
    public LayerMask groundLayer;
    public GameObject target;
    public HpGuage hpGuage;
    public Rigidbody2D rigid;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Vector2 patrolVec;
    public bool isWaited;

    public readonly int IDLE_HASH = Animator.StringToHash("M_Idle");
    public readonly int WALK_HASH = Animator.StringToHash("M_Walk");


    Coroutine attackCor;
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        patrolVec = Vector2.left;
        StateMachineInit();
    }

    private void StateMachineInit()
    {
        //Estate가 아니라 다른 enum사용시 stateMachine을 바꿔줘야함
        stateMachine = new StateMachine();
        stateMachine.stateDic.Add(EState.Idle,new Monster_Idle(this));
        stateMachine.stateDic.Add(EState.Patrol, new Monster_Patrol(this));
        stateMachine.CurState = stateMachine.stateDic[EState.Patrol];
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    IEnumerator CorAttack()
    {
        Attack();
        yield return new WaitForSeconds(1f);
        attackCor = null;
    }

    public void Attack()
    {
        IDamagable setTarget = AttackSearch();
        if (setTarget != null)
        {
            setTarget.TakeDamage(10);
        }
    }

    private IDamagable AttackSearch()
    {
        if (target != null)
        {
            return target.GetComponent<IDamagable>();
        }

        return null;
    }

    public void TakeDamage(int amount)
    {
        hp = Mathf.Max(0, hp - amount);
        SetHpGuage();
    }

    public void SetHpGuage()
    {
        float percent = hp / (float)maxHp;
        hpGuage.GetGuage(percent);
    }
}
