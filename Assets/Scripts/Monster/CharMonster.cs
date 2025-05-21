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


    private bool changeWalk = true;
    private float count;
    [SerializeField] private float countDelay;

    [SerializeField] private LayerMask wallLayer;

    public int rand;
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
            changeWalk = true;
            CanMoving = true;
        }
        else
        {
            count -= Time.deltaTime;
            changeWalk = false;
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
            rand = Random.Range(0, 2);
            if (rand == 0)
            {
                spriteRenderer.flipX = true;
                surfaceEffector.speed = -moveSpeed;
                IsWalk = true;
                count = countDelay;
            }
            else
            {
                spriteRenderer.flipX = false;
                surfaceEffector.speed = moveSpeed;
                IsWalk = true;
                count = countDelay;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == wallLayer)
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
