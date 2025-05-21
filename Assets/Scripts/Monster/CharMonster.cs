using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CharMonster : Monster
{
    private bool CanMoving = true;

    [SerializeField] private float moveSpeed;

    
    private readonly int IDLE_HASH = Animator.StringToHash("M_Idle");
    private readonly int WALK_HASH = Animator.StringToHash("M_Walk");
    private Animator animator;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] SurfaceEffector2D surfaceEffector;

    private bool changeWalk = false;
    private float count;
    [SerializeField] private float countDelay;

    [SerializeField] private LayerMask wallLayer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        OnWalkChanged += SetWalk;
    }

    private void OnDisable()
    {
        OnWalkChanged -= SetWalk;
    }

    private void Update()
    {
        if (count <= 0)
        {
            CanMoving = true;
        }
        else
        {
            count -= Time.deltaTime;
        }

        Move();
    }

    private void Move()
    {
        if (!CanMoving)
        {
            return;
        }

        
        if (changeWalk)
        {
            spriteRenderer.flipX = true;
            surfaceEffector.speed = -moveSpeed;
            IsWalk = true;
        }
        else
        {
            spriteRenderer.flipX = false;
            surfaceEffector.speed = moveSpeed;
            IsWalk = true;
        }
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            Stop();
        }
    }

    private void Stop()
    {
        CanMoving = false;
        IsWalk = false;
        spriteRenderer.flipX = !spriteRenderer.flipX;
        surfaceEffector.speed = 0;
        changeWalk = !changeWalk;
        count = countDelay;
    }


    private void SetWalk(bool value)
    {
        if (value)
        {
            animator.Play(WALK_HASH);
        }
        else
        {
            animator.Play(IDLE_HASH);
        }
    }
}
