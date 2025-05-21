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

    private bool isWalk;
    public bool IsWalk { get { return isWalk; } set { isWalk = value; OnWalk?.Invoke(isWalk); } }
    public event Action<bool> OnWalk;
}
