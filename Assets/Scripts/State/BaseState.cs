using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������̽� ���� �߻�Ŭ���� Ŭ���� �ٰ���
// BaseState ��� �������� Ȯ���ϰ� ����ֱ� ���� abstract���
// enemy,player �� �� �� �ֱ� ������ ���� ���ص���
public abstract class BaseState
{
    public bool HasPhysics;

    // ���°� ���۵� ��
    public abstract void Enter();

    // �ش� ���¿��� ������ ���
    public abstract void Update();

    //������� �ʴ� ���µ鵵 �ֱ� ������ �����Լ� ���
    public virtual void FixedUpdate() { }

    // ���°� ������
    public abstract void Exit();
}

public enum EState
{
    Idle, Walk, Jump, Attack, Patrol, Ladder
}