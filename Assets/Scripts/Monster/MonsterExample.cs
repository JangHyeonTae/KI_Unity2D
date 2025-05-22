using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterExample : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rigid;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 patrolVec;
    private bool isWaited;

    private readonly int IDLE_HASH = Animator.StringToHash("M_Idle");
    private readonly int WALK_HASH = Animator.StringToHash("M_Walk");
    
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        patrolVec = Vector2.left;

    }

    private void Update()
    {
        Patrol();
    }
    void FixedUpdate()
    {
        if (!isWaited)
            rigid.velocity = patrolVec * moveSpeed;
    }
    private void Patrol()
    {
        //주의점 transform.position + new Vector2()안됨
        // -> tranfrom.position은 Vector3이기 떄문
        Vector2 rayOrigin = transform.position + new Vector3(patrolVec.x, 0);
        Debug.DrawRay(rayOrigin, Vector2.down * 3f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 3f, groundLayer);
        if (hit.collider == null)
        {
            StartCoroutine(CoTurnBack());
        }
    }

    private IEnumerator CoTurnBack()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
        if(spriteRenderer.flipX)
        {
            patrolVec = Vector2.left;
        }
        else
        {
            patrolVec = Vector2.right;
        }
        animator.Play(IDLE_HASH);
        isWaited = true;
        rigid.velocity = Vector2.zero;

        yield return new WaitForSeconds(3f);

        isWaited = false;
        animator.Play(WALK_HASH);
    }

}
