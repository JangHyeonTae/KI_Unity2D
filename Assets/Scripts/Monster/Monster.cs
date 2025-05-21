using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int MaxHp;

    [Header("Event Settings")]
    private int currentHp;
    public int CurrentHp { get { return currentHp; } set { currentHp = value;  OnHpChange?.Invoke(currentHp); } }
    public event Action<int> OnHpChange;

    private bool isWalk;
    public bool IsWalk { get { return isWalk; } set { isWalk = value; OnWalkChanged?.Invoke(isWalk); } }
    public event Action<bool> OnWalkChanged; 
}
