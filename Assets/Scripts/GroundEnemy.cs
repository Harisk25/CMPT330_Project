using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;

    [Header("PlayerCheck")]
    public Transform playerCheckPos;
    public Vector2 playerCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask playerHitBoxLayer;
    bool inContactWithPlayer;

    [Header("EnemyMovement")]
    public GameObject pointA; // Point A and Point B are the given area that the ground enemy will move
    public GameObject pointB;
    public float speed;
    float horizontalMovement;
    private bool isWalking = false;
    bool isFacingRight = false;

    [Header("DamagePlayer")]
    public Transform playerDetectionPos;
    public Vector2 playerDetectionSize = new Vector2(0.5f, 0.05f);
    public LayerMask playerLayer;
    bool playerDetected;
    bool isAttacking = false;
    bool isWaiting = false;
    float attackTime = 0.5f; // attack timer to play out bite animation
    float attackTimer;
    float waitTime = 0.8f; // wait timer to wait for next attack
    float waitTimer;


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
        PlayerDetectionCheck();
        Attack();
        if (playerDetected == false)
        {
            Move();
        }
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("isWaiting", isWaiting);

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
     *  Attack will set the booleans to true or false so that the enemy can play out the attack animations
     */
    private void Attack()
    {
        if(playerDetected == true)
        {
            if (isWaiting == false)
            {
                isAttacking = true;
                isWalking = false;
                attackTimer = attackTime;
            }

        }
        else
        {
            isWalking = true;
            isAttacking = false;
        }
    }
    /*
     * ProcessAttack will let the ground enemy play out the attack animation as well as have a wait timer for the next attack;
     */
    private void ProcessAttack()
    {
        if (attackTimer > 0f) // attack timer has started
        {
            attackTimer -= Time.deltaTime;
        }
        else if (attackTimer <= 0f && isAttacking == true)
        {
            isWaiting = true;
            waitTimer = waitTime;

        }
        if (waitTimer > 0f) // attack timer has started
        {
            waitTimer -= Time.deltaTime;
        }
        else if (waitTimer <= 0f && isAttacking == true)
        {
            isWaiting = false;
        }
        else
        {
            isWaiting = false;
            isWalking = true;
        }
    }
    /*
     * PlayerCheck will see if the player is hitting the enemy.
     */
    private void PlayerCheck()
    {
        if (Physics2D.OverlapBox(playerCheckPos.position, playerCheckSize, 0, playerHitBoxLayer))
        {
            inContactWithPlayer = true;
        }
        else
        {
            inContactWithPlayer = false;
        }
    }
    
    /*
     * PlayerDetection will check if player is within attacking range
     */
    private void PlayerDetectionCheck()
    {
        if(Physics2D.OverlapBox(playerDetectionPos.position, playerDetectionSize, 0, playerLayer))
        {
            playerDetected = true;
        }
        else
        {
            playerDetected = false;
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
        //PatrolArea
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
        //DetectionBox
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerDetectionPos.position, playerDetectionSize);
    }
}
