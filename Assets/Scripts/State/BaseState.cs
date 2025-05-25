using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인터페이스 가상 추상클래스 클래스 다가능
// BaseState 어떻게 동작할지 확실하게 잡아주기 위해 abstract사용
// enemy,player 다 될 수 있기 때문에 여기 안해도됨
public abstract class BaseState
{
    public bool HasPhysics;

    // 상태가 시작될 때
    public abstract void Enter();

    // 해당 상태에서 동작을 담당
    public abstract void Update();

    //사용하지 않는 상태들도 있기 때문에 가상함수 사용
    public virtual void FixedUpdate() { }

    // 상태가 끝날때
    public abstract void Exit();
}

public enum EState
{
    Idle, Walk, Jump, Attack, Patrol, Ladder
}