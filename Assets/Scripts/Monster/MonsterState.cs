using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState : BaseState
{
    protected Monsters monster;

    public MonsterState(Monsters _monster)
    {
        monster = _monster;
    }

    public override void Enter()
    {
        
    }
    public override void Update()
    {

    }

    public override void FixedUpdate()
    {
        
    }

    public override void Exit()
    {
        
    }
}

public class Monster_Idle : MonsterState
{
    private float waitedTime;
    public Monster_Idle(Monsters _monster) : base(_monster)
    {
        HasPhysics = false;
    }

    public override void Enter()
    {
        monster.rigid.velocity = Vector2.zero;
        monster.animator.Play(monster.IDLE_HASH);
        monster.isWaited = true;
        monster.spriteRenderer.flipX = !monster.spriteRenderer.flipX;
        if (monster.spriteRenderer.flipX)
        {
            monster.patrolVec = Vector2.left;
        }
        else
        {
            monster.patrolVec = Vector2.right;
        }
        waitedTime = 0;
    }

    public override void Update()
    {
        waitedTime += Time.deltaTime;
        if (waitedTime > 3)
        {
            monster.stateMachine.ChangeState(monster.stateMachine.stateDic[EState.Patrol]);
        }
    }

    public override void Exit() {}
}

public class Monster_Patrol : MonsterState
{
    public Monster_Patrol(Monsters _monster) : base(_monster)
    {
        HasPhysics = true;
    }

    public override void Enter()
    {
        monster.animator.Play(monster.WALK_HASH);
    }

    public override void Update()
    {
        Vector2 rayOrigin = monster.transform.position + new Vector3(monster.patrolVec.x, 0);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 3f, monster.groundLayer);
        if (hit.collider == null)
        {
            monster.stateMachine.ChangeState(monster.stateMachine.stateDic[EState.Idle]);
        }
    }

    public override void FixedUpdate()
    {
        monster.rigid.velocity = monster.patrolVec * monster.moveSpeed;
    }
}