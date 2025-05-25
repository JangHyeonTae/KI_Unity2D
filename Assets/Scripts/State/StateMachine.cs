using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    // 상태를 가지고 있을 거임
    public Dictionary<EState, BaseState> stateDic;

    // 각 상태를 받아서 조건에 따라 상태를 전이시켜 줄거임
    public BaseState CurState;
    public StateMachine()
    {
        stateDic = new Dictionary<EState, BaseState>();
    }
    
    public void ChangeState(BaseState changedState)
    {
        if (CurState == changedState) return;
        CurState.Exit(); // 이전의 상태 나옴
        CurState = changedState; // 상태 바꾸기
        CurState.Enter(); // 바뀐 상태 진입
    }

    // 각 상태의 Enter, Update, Exit ~~를 실행시켜 줄것임
    public void Update() => CurState.Update();

    public void FixedUpdate()
    {
        if(CurState.HasPhysics)
        CurState.FixedUpdate();
    }
}
