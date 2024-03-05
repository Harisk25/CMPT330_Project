using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;

    [Header("PlayerCheck")]
    public Transform playerCheckPos;
    public Vector2 playerCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask playerLayer;
    bool inContactWithPlayer;

    [Header("EnemyMovement")]
    public float speed;
    float horizontalMovement;
    private bool isWalking = false;
    bool isFacingRight = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerCheck();
        GettingHit();
        Move();

        anim.SetBool("isWalking", isWalking);
    }

    /*
     * GettingHit will reduce HP and trigger death if HP == 0
     */
    private void GettingHit()
    {
        if (inContactWithPlayer == true)
        {
            Debug.Log("Player Hitting Me! ");
        }
    }

    /*
     * This is enemy movement
     */
    private void Move()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
            if (isFacingRight == false)
            {
                Flip();
                isFacingRight = true;
            }
            isWalking = true;
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
            if (isFacingRight == true)
            {
                Flip();
                isFacingRight = false;
            }
            isWalking = true;
        }
        if(Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            currentPoint = pointA.transform;
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            currentPoint = pointB.transform;
        }

    }

    /*
     * PlayerCheck will see if the player is hitting the enemy.
     */
    private void PlayerCheck()
    {
        if (Physics2D.OverlapBox(playerCheckPos.position, playerCheckSize, 0, playerLayer))
        {
            inContactWithPlayer = true;
        }
        else
        {
            inContactWithPlayer = false;
        }
    }

    /*
     * Flip just flips the sprite.
     */
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 ls = transform.localScale;
        ls.x *= -1f;
        transform.localScale = ls;
    }

    /*
     * OnDrawGizmosSelected draws our GroundCheck and WallCheck hit boxes so we can see how big they are.
     */
    private void OnDrawGizmosSelected()
    {
        //PlayerCheck
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(playerCheckPos.position, playerCheckSize);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}
