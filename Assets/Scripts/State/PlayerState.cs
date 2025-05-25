using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// �÷��̾� �����̱� ������ ���⼭ player Ŭ���� �߰�
// ���� �ؽ��� ��� ���Ұ�� �ִ��� playerState����
// bool�� ���� ���� �ؿ��� bool�� �����ؾ���
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

    //anyState�� Update���� ���۽�ų����
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

// ���� ���� ��������� ����
// ���⼭ Animator�� ������ �� ���� ������ state����
// ������ �� �ִ� ����� �ʿ���
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
        // Idle ����
        if (Mathf.Abs(player.inputX) < 0.1f)
        {
            player.stateMachine.ChangeState(player.stateMachine.stateDic[EState.Idle]);
        }
        // ���� ������ ���� ��ȯ
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