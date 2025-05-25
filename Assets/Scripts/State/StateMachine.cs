using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    // ���¸� ������ ���� ����
    public Dictionary<EState, BaseState> stateDic;

    // �� ���¸� �޾Ƽ� ���ǿ� ���� ���¸� ���̽��� �ٰ���
    public BaseState CurState;
    public StateMachine()
    {
        stateDic = new Dictionary<EState, BaseState>();
    }
    
    public void ChangeState(BaseState changedState)
    {
        if (CurState == changedState) return;
        CurState.Exit(); // ������ ���� ����
        CurState = changedState; // ���� �ٲٱ�
        CurState.Enter(); // �ٲ� ���� ����
    }

    // �� ������ Enter, Update, Exit ~~�� ������� �ٰ���
    public void Update() => CurState.Update();

    public void FixedUpdate()
    {
        if(CurState.HasPhysics)
        CurState.FixedUpdate();
    }
}
