using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("Event ")]
    private bool isJumped;
    public bool IsJumped { get { return isJumped; } set { isJumped = value; OnJumped?.Invoke(isJumped); } }
    public event Action<bool> OnJumped;

    private bool isIdle;
    public bool IsIdle { get { return isIdle; } set { isIdle = value; OnIdle?.Invoke(isIdle); } }
    public event Action<bool> OnIdle;

    private bool isWalk;
    public bool IsWalk { get { return isWalk; } set { isWalk = value; OnWalk?.Invoke(isWalk); } }
    public event Action<bool> OnWalk;

    private bool isAttack;
    public bool IsAttack { get { return isAttack; } set { isAttack = value; OnAttack?.Invoke(isAttack); } }
    public event Action<bool> OnAttack;
}
