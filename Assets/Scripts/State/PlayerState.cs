using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 플레이어 상태이기 때문에 여기서 player 클래스 추가
// 만약 해쉬를 사용 안할경우 최대한 playerState에서
// bool값 변경 없이 밑에서 bool값 조정해야함
public class PlayerState : BaseState
{
    protected Player player;
    

    public PlayerState(Player _player)
    {
        player = _player;
    }
    public override void Enter()
    {
        
    }

    public override void FixedUpdate()
    {
        player.rigid.velocity = new Vector2(player.inputX * player.moveSpeed, player.rigid.velocity.y);

    }

    //anyState는 Update에서 동작시킬거임
    public override void Update()
    {
        if (player.isLaddered && Mathf.Abs(player.inputY) > 0.1f)
        {
            player.IsTrig = true;
            player.stateMachine.ChangeState(player.stateMachine.stateDic[EState.Ladder]);
        }

        if (player.isJumped && player.isGrounded)
        {
            player.stateMachine.ChangeState(player.stateMachine.stateDic[EState.Jump]);
        }

        if (player.canAttacked && !player.isAttacked)
        {
            player.stateMachine.ChangeState(player.stateMachine.stateDic[EState.Attack]);
        }
    }

    public override void Exit()
    {

    }
}

// 원래 따로 지정해줘야 좋음
// 여기서 Animator를 가져올 수 없기 때문에 state에서
// 가져올 수 있는 방법이 필요함
public class Player_Idle : PlayerState
{
    public Player_Idle(Player _player) : base(_player)
    {
        HasPhysics = false;
    }
    public override void Enter()
    {
        player.animator.Play(player.IDLE_HASH);
        player.rigid.velocity = Vector2.zero;
    }

    public override void Update()
    {
        base.Update();
        if (Mathf.Abs(player.inputX) > 0.1f)// || Mathf.Abs(player.inputY)>0.1f)
        {
            player.stateMachine.ChangeState(player.stateMachine.stateDic[EState.Walk]);
        }
    }
    public override void Exit() {}
}

public class Player_Walk : PlayerState
{
    public Player_Walk(Player _player) : base(_player)
    {
        HasPhysics = true;
    }
    public override void Enter()
    {
        player.animator.Play(player.WALK_HASH);
    }

    public override void Update()
    {
        base.Update();
        // Idle 전이
        if (Mathf.Abs(player.inputX) < 0.1f)
        {
            player.stateMachine.ChangeState(player.stateMachine.stateDic[EState.Idle]);
        }
        // 왼쪽 오른쪽 방향 전환
        //player.spriteRenderer.flipX = player.inputX < 0;
        if (player.inputX < 0)
        {
            player.spriteRenderer.flipX = true;
        }
        else
        {
            player.spriteRenderer.flipX = false;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit() { }
}

public class Player_Jump : PlayerState
{
    public Player_Jump(Player _player) : base(_player)
    {
        HasPhysics = true;
    }
    public override void Enter()
    {
        player.animator.Play(player.JUMP_HASH);
        player.rigid.AddForce(Vector2.up * player.jumpPower, ForceMode2D.Impulse);
        player.isGrounded = false;
        player.isJumped = false;
    }

    public override void Update()
    {
        base.Update();
        if (player.isGrounded)
        {
            player.stateMachine.ChangeState(player.stateMachine.stateDic[EState.Idle]);
        }


        if (player.inputX < 0)
        {
            player.spriteRenderer.flipX = true;
        }
        else
        {
            player.spriteRenderer.flipX = false;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit() { }
}

public class Player_Attack : PlayerState
{
    public Player_Attack(Player _player) : base(_player)
    {
        HasPhysics = false;
    }
    public override void Enter()
    {
        player.animator.Play(player.ATTACK_HASH);
        player.AttackAnim();
        player.isAttacked = true;
    }

    public override void Update()
    {
        if (!player.isAttacked)
        {
            player.stateMachine.ChangeState(player.stateMachine.stateDic[EState.Idle]);
        }
    }

    public override void Exit() { }
}

public class Player_Ladder : PlayerState
{
    public Player_Ladder(Player _player) : base(_player)
    {
        HasPhysics = true;
    }

    public override void Enter()
    {
         player.animator.Play(player.LADDER_HASH);
        
    }

    public override void Update()
    {
        if (Mathf.Abs(player.inputY) < 0.1f || !player.isLaddered)
        {
            Debug.Log("Out");
            player.stateMachine.ChangeState(player.stateMachine.stateDic[EState.Idle]);
            player.IsTrig = false;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.rigid.velocity = new Vector2(player.rigid.velocity.x, player.inputY * player.upSpeed);
    }

    public override void Exit() { }

}