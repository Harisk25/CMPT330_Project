using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform playerLocation;
    public Transform bossLocation;
    Rigidbody2D rb;
    public BoxCollider2D boxCollider;
    public Animator animator;
    public CapsuleCollider2D hitBoxSword;
    public LayerMask playerLayer;
    public bool playerEnter = false;
    bool isMoving = false;
    public int health = 24;
    bool inContactWithPlayer = false;
    bool enemyInvincible = false;
    float enemyInvincbleTimer;
    public float enemyInvincbleTime = 4f;
    bool isDead = false;
    public int stage = 1;
    public int fStage2 = 1;
    public int fStage3 = 1;
    int setAttack = 0;
    int cycle = 0;
    int maxCycles = 1;
    int wait = 0;
    float waitTime = 2f;
    float waitTimer;

    [Header("Attack1")]
    public Transform playerDetectionPosA1;
    public Vector2 playerDetectionSizeA1 = new Vector2(0.5f, 0.05f);
    bool playerDetected;
    int numberOfAttacks = 0;
    
    [Header("SlideAttack")]
    bool noFlip = false;
    int numberOfSlides = 2;
    bool startSliding = false;
    int slideTarget = 0;
    public float speed = 10f;
    bool facingRight = true;
    float slideWaitTime = 0.8f;
    float slideWaitTimer;
    int slideWait = 0;
    float slideWaitTimeOverall = 1.5f;
    float slideWaitTimerOverall;
    int slideWaitOverall = 0;

    [Header("AirAttack")]
    bool needsTP = true;
    public Transform tracker;
    public Rigidbody2D trackerRb;
    public SpriteRenderer trackerSprite;
    float trackerTime = 5f;
    float trackerTimer;
    float airTime = 1f;
    float airTimer;
    bool airAttack = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        trackerSprite.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerEnter == true)
        {
            if (playerLocation.position.x < bossLocation.position.x && facingRight == true && noFlip == false)
            {
                Flip();
                facingRight = false;
            }
            else if (playerLocation.position.x > bossLocation.position.x && facingRight == false && noFlip == false)
            {
                Flip();
                facingRight = true;
            }
            if (wait == 0 && cycle == 0)
            {
                if (setAttack == 0)
                {
                    AttackOne();
                    
                }
                else if (setAttack == 1)
                {
                    AttackTwo();
                    
                }
            }
            else if (cycle == 1 && wait == 0 && stage == 2)
            {
                SlideAttack();
                ProcessSlideWait();
                ProcessSlideWaitOverall();
                FlipSlide();
            }
            else if (cycle == 2 && wait == 0 && stage == 3)
            {
                AirAttack();
                ProcessTracker();
                ProcessAirTime();
            }
            else
            {

                ProcessWait();
            }
            
            if(numberOfAttacks == 3 && stage == 2)
            {
                cycle++;
                numberOfAttacks = 0;
            }
            else if(numberOfAttacks == 3)
            {
                numberOfAttacks = 0;
            }
            if (cycle >= maxCycles)
            {
                cycle = 0;
            }
            
            ProcessStages();
            PlayerDetectionCheck();
            ProcessEnemyInvincble();
            GettingHit();
            animator.SetFloat("Moving", rb.velocity.magnitude);
            Debug.Log(setAttack);
        }
    }
    void ProcessStages()
    {
        if (health <= 18 && health > 12)
        {
            stage = 2;

            if (fStage2 == 1)
            {
                setAttack = 0;
                enemyInvincbleTimer = enemyInvincbleTime + 1f;
                fStage2 = 0;
                cycle = 1;
                numberOfAttacks = 0;
                maxCycles = 2;
            }

        } 
        else if (health <= 12)
        {
            stage = 3;

            if (fStage3 == 1)
            {
                setAttack = 0;
                enemyInvincbleTimer = enemyInvincbleTime + 1f;
                fStage3 = 0;
                cycle = 2;
                numberOfAttacks = 0;
                maxCycles = 3;
            }
        }
    }

    private void GettingHit()
    {
        if (inContactWithPlayer == true && enemyInvincible == false)
        {
            health--;
            enemyInvincible = true;
            inContactWithPlayer = false;
            enemyInvincbleTimer = enemyInvincbleTime;
            rb.velocity = new Vector2(0, 0);
            if (health <= 0)
            {
                isDead = true;
                enemyInvincbleTimer += 0.5f;
                rb.gravityScale = 0;
            }
        }
    }

    /*
    * ProcessEnemyInvincble makes it so that the enemy cant get hit for a few seconds after getting hit.
    */
    private void ProcessEnemyInvincble()
    {
        if (enemyInvincible == true && enemyInvincbleTimer > 0f)
        {
            enemyInvincbleTimer -= Time.deltaTime;
        }
        else if (enemyInvincbleTimer <= 0f)
        {
            enemyInvincible = false;
            if (isDead == true)
            {
                Destroy(gameObject);
            }
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player HitBox"))
        {
            inContactWithPlayer = true;
        }

    }

    void AttackOne()
    {
        Vector2 target = (playerLocation.position - bossLocation.position).normalized;
        Vector2 move = new Vector2(target.x, 0) * speed;
        rb.velocity = move;
        if(playerDetected == true)
        {
            if (stage >= 2)
            {
                setAttack = 1;
            }
            rb.velocity = new Vector2(0,0);
            Debug.Log("called");
            animator.SetTrigger("Attack1");
            numberOfAttacks++;
            waitTimer = waitTime;
            wait = 1;
        }
    }

    void AttackTwo()
    {
        Vector2 target = (playerLocation.position - bossLocation.position).normalized;
        Vector2 move = new Vector2(target.x, 0) * speed;
        rb.velocity = move;
        if (playerDetected == true)
        {
            setAttack = 0;
            rb.velocity = new Vector2(0, 0);
            Debug.Log("called");
            animator.SetTrigger("Attack2");
            numberOfAttacks++;
            waitTimer = waitTime;
            wait = 1;
        }
    }

    private void PlayerDetectionCheck()
    {
        if (Physics2D.OverlapBox(playerDetectionPosA1.position, playerDetectionSizeA1, 0, playerLayer))
        {
            playerDetected = true;
        }
        else
        {
            playerDetected = false;
        }
    }

    private void ProcessWait()
    {
        if (waitTimer > 0f) // attack timer has started
        {
            waitTimer -= Time.deltaTime;
        }
        else if (waitTimer <= 0f) // attack timer finished, and player hasnt attacked yet, start reset timer
        {
            wait = 0;

        }
    }

    private void ProcessSlideWait()
    {
        if (slideWaitTimer > 0f) // attack timer has started
        {
            slideWaitTimer -= Time.deltaTime;
        }
        else if (slideWaitTimer <= 0f) // attack timer finished, and player hasnt attacked yet, start reset timer
        {
            slideWait = 0;

        }
    }
    private void ProcessSlideWaitOverall()
    {
        if (slideWaitTimerOverall > 0f) // attack timer has started
        {
            slideWaitTimerOverall -= Time.deltaTime;
        }
        else if (slideWaitTimerOverall <= 0f) // attack timer finished, and player hasnt attacked yet, start reset timer
        {
            slideWaitOverall = 0;

        }
    }
    private void FlipSlide()
    {
        if (bossLocation.position.x <= -319 && facingRight == false)
        {
            Flip();
            facingRight = true;
        }
        else if (bossLocation.position.x >= -285 && facingRight == true)
        {
            Flip();
            facingRight = false;
        }
    }

    private void SlideAttack()
    {
        Vector2 target;
        Vector2 move;

        //Move Boss to location //-319
        if (slideWaitOverall == 0)
        {
            if (startSliding == false)
            {
                if (-319 < bossLocation.position.x && facingRight == true)
                {
                    Flip();
                    facingRight = false;
                }
                target = (new Vector3(-319, 20, 45) - bossLocation.position).normalized;
                move = new Vector2(target.x, 0) * speed;
                rb.velocity = move;
                if (bossLocation.position.x <= -319)
                {
                    rb.velocity = new Vector2(0, 0);
                    animator.Play("Idle");
                    slideTarget = -285;
                    startSliding = true;
                    noFlip = true;
                    slideWaitOverall = 1;
                    slideWaitTimerOverall = slideWaitTimeOverall;
                }
            }
            else
            {
                if (numberOfSlides != 0 && slideTarget == -285)
                {
                    target = (new Vector3(-285, 20, 45) - bossLocation.position).normalized;
                    move = new Vector2(target.x, 0) * speed * 5;
                    rb.velocity = move;
                    if (slideWait == 0)
                    {
                        animator.SetTrigger("Sliding");
                        slideWait = 1;
                        slideWaitTimer = slideWaitTime;
                    }
                    if (bossLocation.position.x >= -285)
                    {
                        rb.velocity = new Vector2(0, 0);
                        animator.Play("Idle");
                        slideTarget = -319;
                        numberOfSlides--;
                        slideWaitOverall = 1;
                        slideWaitTimerOverall = slideWaitTimeOverall;
                    }
                }
                else if (numberOfSlides != 0 && slideTarget == -319)
                {
                    target = (new Vector3(-319, 20, 45) - bossLocation.position).normalized;
                    move = new Vector2(target.x, 0) * speed * 5;
                    rb.velocity = move;
                    if (slideWait == 0)
                    {
                        animator.SetTrigger("Sliding");
                        slideWait = 1;
                        slideWaitTimer = slideWaitTime;
                    }
                    if (bossLocation.position.x <= -319)
                    {
                        rb.velocity = new Vector2(0, 0);
                        animator.Play("Idle");
                        slideTarget = -285;
                        numberOfSlides--;
                        slideWaitOverall = 1;
                        slideWaitTimerOverall = slideWaitTimeOverall;
                    }
                }
                if (numberOfSlides == 0)
                {
                    noFlip = false;
                    cycle++;
                    startSliding = false;
                    numberOfSlides = 2;
                    waitTimer = waitTime;
                    wait = 1;

                }
            }
        }
    }

    private void ProcessTracker()
    {
        if (trackerTimer > 0f) // attack timer has started
        {
            trackerTimer -= Time.deltaTime;
        }
    }
    private void ProcessAirTime()
    {
        if (airTimer > 0f) // attack timer has started
        {
            airTimer -= Time.deltaTime;
        }
    }
    private void AirAttack()
    {
        // tp to position -261, 49
        if(needsTP == true)
        {
            bossLocation.position = new Vector3(-261, 49, 45);
            needsTP = false;
            trackerSprite.enabled = true;
            trackerTimer = trackerTime;
        }
        if (trackerTimer > 0f)
        {
            Vector2 target = (playerLocation.position - tracker.position).normalized;
            Vector2 move = new Vector2(target.x, 0) * 200;
            trackerRb.velocity = move;
        }
        else if (trackerTimer < 0f && airAttack == false) 
        {
            trackerRb.velocity = new Vector2(0, 0);
            airTimer = airTime;
            airAttack = true;
        }
        if(airAttack == true && airTimer <= 0f)
        {
            bossLocation.position = new Vector3(tracker.position.x, 35, 45);
            animator.SetTrigger("AirAttack");
            rb.gravityScale = 10f;
            airAttack = false;
            cycle++;
            trackerSprite.enabled = false;
            wait = 1;
            waitTimer = waitTime + 2f;

        }

    }

    /*
     * Flip just flips the sprite.
    */
    private void Flip()
    {
        Vector3 ls = transform.localScale;
        ls.x *= -1f;
        transform.localScale = ls;
    }

    private void OnDrawGizmosSelected()
    {
        //DetectionBox
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerDetectionPosA1.position, playerDetectionSizeA1);
    }
}