using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("PlayerStats")]
    public Transform playerLocation;
    public Transform bossLocation;
    Rigidbody2D rb;
    public SimpleFlash flashDamage;
    public BoxCollider2D boxCollider;
    public BoxCollider2D physicBox;
    public Animator animator;
    public CapsuleCollider2D hitBoxSword;
    public LayerMask playerLayer;
    public bool playerEnter = false;
    public GameObject door;
    public GameObject key;
    public int health = 24;
    bool dead = false;
    bool inContactWithPlayer = false;
    bool enemyInvincible = false;
    float enemyInvincbleTimer;
    public float enemyInvincbleTime = 4f;
    public int stage = 1;
    public int fStage2 = 1;
    public int fStage3 = 1;
    int setAttack = 0;
    int cycle = 0;
    int maxCycles = 1;
    int wait = 0;
    float waitTime = 2f;
    float waitTimer;

    [Header("UI")]
    public Animator animatorUI;

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
    float airTime = 0.1f;
    float airTimer;
    float animTime = 1f;
    float animTimer;
    bool anim = true;
    bool airAttack = false;

    public AudioClip hurtSoundEffect;
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
            if (dead == false)
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
                    else if (setAttack == 2)
                    {
                        AttackThree();

                    }
                }
                else if (cycle == 1 && wait == 0 && stage >= 2)
                {
                    SlideAttack();
                    ProcessSlideWait();
                    ProcessSlideWaitOverall();
                    FlipSlide();
                }
                else if (cycle == 2 && wait == 0 && stage >= 3)
                {

                    AirAttack();
                    ProcessTracker();
                    ProcessAirTime();
                }
                else
                {

                    ProcessWait();
                }

                if (numberOfAttacks == 3 && stage >= 2)
                {
                    cycle++;
                    numberOfAttacks = 0;
                }
                else if (numberOfAttacks >= 3)
                {
                    numberOfAttacks = 0;
                }
                if (cycle >= maxCycles)
                {
                    cycle = 0;
                }
                ProcessAnimWait();
                ProcessStages();
                PlayerDetectionCheck();
                ProcessEnemyInvincble();
                GettingHit();
                animator.SetFloat("Moving", rb.velocity.magnitude);
                animator.SetBool("isDead", dead);
            }
            else
            {
                animatorUI.SetTrigger("3H");
                rb.bodyType = RigidbodyType2D.Static;
                physicBox.enabled = false;
                boxCollider.enabled = false;
                hitBoxSword.enabled = false;
                door.SetActive(true);
                

            }
        }
    }
    void ProcessStages()
    {
        if (health <= 18 && health > 12)
        {
            stage = 2;

            if (fStage2 == 1)
            {
                animatorUI.SetTrigger("1H");
                setAttack = 0;
                enemyInvincbleTimer = enemyInvincbleTime + 1f;
                fStage2 = 0;
                cycle = 1;
                numberOfAttacks = 0;
                maxCycles = 2;
            }

        }
        else if (health <= 12 && health > 0)
        {
            stage = 3;

            if (fStage3 == 1)
            {
                animatorUI.SetTrigger("2H");
                setAttack = 0;
                enemyInvincbleTimer = enemyInvincbleTime + 1f;
                fStage3 = 0;
                cycle = 2;
                numberOfAttacks = 0;
                maxCycles = 3;
                noFlip = false;
                startSliding = false;
                numberOfSlides = 2;
            }
        }else if( health <= 0)
        {
            dead = true;
            key.SetActive(true);
            animator.SetTrigger("Dead");
        }
    }

    private void GettingHit()
    {
        if (inContactWithPlayer == true && enemyInvincible == false)
        {
            flashDamage.Flash();
            health--;
            enemyInvincible = true;
            inContactWithPlayer = false;
            enemyInvincbleTimer = enemyInvincbleTime;
            PlayHurtSound();
            rb.velocity = new Vector2(0, 0);
            if (health <= 0)
            {
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
            inContactWithPlayer = false;
            enemyInvincbleTimer -= Time.deltaTime;
        }
        else if (enemyInvincbleTimer <= 0f)
        {
            enemyInvincible = false;
            
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
        if (playerDetected == true)
        {
            if (stage >= 2)
            {
                setAttack = 1;
            }
            rb.velocity = new Vector2(0, 0);
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
            if (stage >= 3)
            {
                setAttack = 2;
            }
            else
            {
                setAttack = 0;
            }
            rb.velocity = new Vector2(0, 0);
            Debug.Log("called");
            animator.SetTrigger("Attack2");
            numberOfAttacks++;
            waitTimer = waitTime;
            wait = 1;
        }
    }
    void AttackThree()
    {
        Vector2 target = (playerLocation.position - bossLocation.position).normalized;
        Vector2 move = new Vector2(target.x, 0) * speed;
        rb.velocity = move;
        if (playerDetected == true)
        {
            setAttack = 0;
            rb.velocity = new Vector2(0, 0);
            Debug.Log("called");
            animator.SetTrigger("Attack3");
            waitTimer = waitTime;
            numberOfAttacks++;
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
    private void ProcessAnimWait()
    {
        if (animTimer > 0f) // attack timer has started
        {
            animTimer -= Time.deltaTime;
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
        if(anim == true)
        {
            rb.velocity = new Vector2(0, 0);
            animTimer = animTime;
            animator.SetTrigger("Pray");
            anim = false;
        }
        // tp to position -261, 49
        if (animTimer < 0f)
        {
            if (needsTP == true)
            {
                bossLocation.position = new Vector3(-261, 49, 45);
                needsTP = false;
                trackerSprite.enabled = true;
                trackerTimer = trackerTime;
            }
            else if (trackerTimer > 0f)
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
            else if (airAttack == true && airTimer <= 0f)
            {
                bossLocation.position = new Vector3(tracker.position.x, 35, 45);
                animator.SetTrigger("AirAttack");
                rb.gravityScale = 15f;
                airAttack = false;
                cycle++;
                trackerSprite.enabled = false;
                needsTP = true;
                wait = 1;
                waitTimer = waitTime + 1f;
                anim = true;
            }
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

    public void PlayHurtSound()
    {
        SoundFXManager.instance.PlaySoundFXClip(hurtSoundEffect, transform, 0.5f);
    }
}