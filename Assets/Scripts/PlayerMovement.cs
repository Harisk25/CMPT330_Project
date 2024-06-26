using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public CapsuleCollider2D hitBox;
    public BoxCollider2D playerBox;
    bool isFacingRight = true;

    [Header ("Player Stats")] //Header adds headers in unity to making sectiosn of  variables popout.
    public int playerHP = 4;
    bool playerInvincible = false;
    float playerInvincbleTimer;
    public float playerInvincbleTime = 0.8f;
    public bool wallPowerupActive = false;
    public bool hasKey = false;
    private bool isDead = false;
    [Header ("Display Player Stats")]
    public int numOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Image wallGlove;
    public Image key;
    [Header("Display Score")]
    public int scoreCount = 0;
    public Text scoreText;


    [Header ("Movement")] 
    public float moveSpeed = 5f;
    float horizontalMovement;
    bool keyDown = false;

    [Header("Jumping")]
    public float jumpPower = 10f;
    public int maxJumps = 1;
    private int jumpsRemaining;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    bool isGrounded;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    [Header("WallCheck")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask wallLayer;

    [Header("EnemyCheck")]
    public Transform enemyCheckPos;
    public Vector2 enemyCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask enemyLayer;
    bool inContactWithEnemy;

    [Header("WallMovement")]
    public float wallSlideSpeed = 2f;
    bool isWallSliding;

    //Wall Jumping
    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.4f;
    float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 10f);

    [Header("Attack")]
    bool isAttacking = false;
    int attackCount = 0;
    float attackTime = 0.4f;
    float attackTimer;
    float attackRestTime = 0.4f;
    float attackRestTimer;

    [Header("ChangeScene")]
    float changeTimer;
    public float changeTime = 2f;

    [Header("Sounds")]
    public AudioClip attackSoundClip;
    public AudioClip jumpSoundClip;
    public AudioClip hurtSoundEffect;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "" + Mathf.Round(scoreCount);
        for (int i = 0; i < hearts.Length; i++) // to display hearts on UI interface for player
        {
            if(i < playerHP) // display hearts based on health
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearts) // number of hearts to display
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
        if(wallPowerupActive == true) // display power up glove when picked up
        {
            wallGlove.enabled = true;
        }
        else
        {
            wallGlove.enabled = false;
        }
        if (hasKey == true) // display key when picked up
        {
            key.enabled = true;
        }
        else
        {
            key.enabled = false;
        }

        if (isDead == false) // check if dead so player cant move after dying
        {
            ProcessGravity();
            GroundCheck();
            ProcessWallSlide();
            ProcessWallJump();
            EnemyCheck();
            ProcessAttack();
            GettingHit();
            ProcessPlayerInvincble();
            if (!isWallJumping)
            {
                rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
                Flip();
            }
        }
        else
        {
            ProcessDeath();
        }
        // set animation variables for animation
        animator.SetFloat("AirSpeedY", rb.velocity.y);
        animator.SetFloat("magnitude", rb.velocity.magnitude);
        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("WallSlide", isWallSliding);
        animator.SetBool("KeyDown(AorD)", keyDown);
    }

    /*
     * ProcessGravity gives gravity to our player. The gravity directly impacts how fast our player falls
     * so that our player doesnt seem like they are floaty. The gravity varibles can be edited in unity.
     */
    private void ProcessGravity()
    {
        //falling gravity
        if (rb.velocity.y < 0 && isGrounded == false) //Player needs to be on the ground and falling (player reached max jump height)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier; //fall faster and faster
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed)); //max fall speed
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }
    
    /*
     * ProcessWallSlide is the method that checks to see if the player is next to a wall while sliding.
     * If the player is besided a wall and is moving towards that wall while falling it will cause wall sliding and reduce fall speed
     */
    private void ProcessWallSlide()
    {
        if (!isGrounded && WallCheck() & horizontalMovement != 0 && wallPowerupActive) //Player must be not grounded and next to a wall and not moving in the x - axis
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed)); //Caps fall rate, this is a reduced speed
        }
        else
        {
            isWallSliding = false;
        }
    }
    
    /*
     * ProcessWallJump allows our player to jump after wall sliding. The player is unable to act (move in the x-axis) until a certian amount of time has passed.
     */
    private void ProcessWallJump() 
    {
        if (isWallSliding) //Player needs to be wall sliding.
        {
            isWallJumping = false; //Player Wall jumped, no longer sliding
            wallJumpDirection = -transform.localScale.x; //Launch in the x direction
            wallJumpTimer = wallJumpTime; //Start buffer timer, prevents contstant wall jumping

            CancelInvoke(nameof(CancelWallJump)); //End of our wall jump, player can input other actions
        }
        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }
    /*
     * ProcessPlayerInvincble makes it so that the player cant get hit for a few seconds after getting hit.
     */
    private void ProcessPlayerInvincble()
    {
        if(playerInvincible == true && playerInvincbleTimer > 0f)
        {
            playerInvincbleTimer -= Time.deltaTime;
        }
        else if (playerInvincbleTimer <= 0f)
        {
            playerInvincible = false;
        }

    }

    /*
     * CancelWallJump is an invoke method that is invoked when a player performs a wall jump
     */
    private void CancelWallJump()
    {
        isWallJumping = false;
    }
    
    /*
     * Move is used to make the player move in the x-axis.
     */
    public void Move(InputAction.CallbackContext context) 
    {
        if (isDead == false)
        {
            if (context.performed) //If statement needed to help fix animation issues when swithing from falling to idle animation.
            {
                horizontalMovement = context.ReadValue<Vector2>().x;
                keyDown = true;
            }
            else
            {
                horizontalMovement = context.ReadValue<Vector2>().x;
                keyDown = false;
            }
        }

    }
    
    /*
     * Jump is used to make our player jump under certain restrictions. Can jump multiple times with the increase in the max jump variable
     * Jump also preforms the wall jump.
     */
    public void Jump(InputAction.CallbackContext context)
    {
        if (isDead == false)
        {
            if (jumpsRemaining > 0) //Check to see if we have jumps remaining
            {
		
                if (context.performed)
                {
                    //Hold down jump button = full height
                    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                    jumpsRemaining--;
                    animator.SetTrigger("Jump");
		    PlayJumpSound();
                }
                else if (context.canceled && rb.velocity.y > 0)
                {
                    //Light tap of jump button = half the height
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                    jumpsRemaining--;
                    animator.SetTrigger("Jump");
                }
            }

            //Wall jump
            if (context.performed && wallJumpTimer > 0f && wallPowerupActive == true)
            {
                isWallJumping = true;
                rb.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
                wallJumpTimer = 0;
                animator.SetTrigger("Jump");
		PlayJumpSound();
                //Force a flip
                if (transform.localScale.x != wallJumpDirection)
                {
                    isFacingRight = !isFacingRight;
                    Vector3 ls = transform.localScale;
                    ls.x *= -1f;
                    transform.localScale = ls;
                }

                Invoke(nameof(CancelWallJump), wallJumpTime + 0.4f); //Wall Jump will last 0.5 seconds and can jump again 0.6 seconds
            }
        }
    }
    
    /*
     * Hit checks if player is in contact with an enemy, if so player takes damage
     */
    private void GettingHit()
    {
        if (inContactWithEnemy == true && playerInvincible == false)
        {
            playerHP--;
            animator.SetTrigger("Hurt");
            playerInvincible = true;
            playerInvincbleTimer = playerInvincbleTime;
            PlayHurtSound();
            if (GetComponent<PlayerInput>().currentControlScheme == "Gamepad")
            {
                RumbleManager.instance.RumblePulse(0.5f, 1f, 0.25f);
            }
            if(playerHP <= 0)
            {
                if (GetComponent<PlayerInput>().currentControlScheme == "Gamepad")
                {
                    RumbleManager.instance.RumblePulse(1f, 1f, 2f);
                }
                animator.SetTrigger("Death");
                isDead = true;
                changeTimer = changeTime;
                playerBox.enabled = false;
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
    }
    private void ProcessDeath()
    {
        if(isDead == true)
        {
            //Send to you died screen, have reset button
            if(changeTimer <= 0f)
            {
                SceneManager.LoadScene("Death");
            }
            else
            {
                changeTimer -= Time.deltaTime;
            }
        }
    }

    /*
     * Attack is allows the player to attack, and triggers animation
     */
    public void Attack(InputAction.CallbackContext context)
    {
        if (isDead == false)
        {
            if (context.performed) // key pressed
            {
                if (attackTimer <= 0f) // ensure attack timer is at 0 to trigger next animation
                {
                    hitBox.enabled = true;
                    isAttacking = true;
                    attackRestTimer = attackRestTime; // set rest timer
                    attackCount++;
                    if (attackCount == 1) // animation 1
                    {
                        attackTimer = attackTime;
                        animator.SetTrigger("Attack1");
                    }
                    else if (attackCount == 2) // animation 2
                    {
                        attackTimer = attackTime;
                        animator.SetTrigger("Attack2");
                    }
                    else if (attackCount == 3) // animation 3
                    {
                        attackTimer = attackTime;
                        animator.SetTrigger("Attack3");
                    }
                }
                if (attackCount == 3) // after 3 attacks reset animation to 1
                {
                    attackCount = 0;
                }
            }
            else if (context.canceled)
            {
                isAttacking = false;

            }
        }
    }

    /*
     * processAttack insures attack animation is played out before player can attack again, sort of a timer
     * to stop animation resets. 
     */
    private void ProcessAttack()
    {
        if(attackTimer > 0f) // attack timer has started
        {
            attackTimer -= Time.deltaTime;
        }
        else if ( attackTimer <= 0f && isAttacking == false) // attack timer finished, and player hasnt attacked yet, start reset timer
        {
            attackRestTimer -= Time.deltaTime;
            hitBox.enabled = false;
            
        }
        if (attackRestTimer <= 0f) // once reset timer at zero reset animation
        {
            attackCount = 0;
            hitBox.enabled = false;
        }
    }
    
    /*
     * GroundCheck makes sure that the player is on the ground.
     * Used for checking if the player can jump.
     */
    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer)) //checks if set box overlaps with ground
        {
            jumpsRemaining = maxJumps; //Resests jump remaining so that the player can jump again.
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    
    /*
     * WallCheck makes sure we are touching a wall to preform a wall slide.
     */
    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer);
    }
    
    /*
     * EnemyCheck checks to see if player is colliding with an enemy
     */
    private void EnemyCheck()
    {
        if (Physics2D.OverlapBox(enemyCheckPos.position, enemyCheckSize, 0, enemyLayer))
        {
            inContactWithEnemy = true;
        }
        else
        {
            inContactWithEnemy = false;
        }
    }
    
    /*
     * Flip just flips the player sprite.
     */
    private void Flip()
    {
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }
    
    /*
     * OnDrawGizmosSelected draws our GroundCheck and WallCheck hit boxes so we can see how big they are.
     */
    private void OnDrawGizmosSelected()
    {
        //Groundcheck hit box
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        //Wallcheck hit box
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
        //Enemycheck hit box
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(enemyCheckPos.position, enemyCheckSize);
    }

    public void PlayAttackSound(){
	SoundFXManager.instance.PlaySoundFXClip(attackSoundClip, transform, 0.6f);     
    }

    public void PlayJumpSound(){
	SoundFXManager.instance.PlaySoundFXClip(jumpSoundClip, transform, 0.6f);     
    }

    public void PlayHurtSound()
    {
        SoundFXManager.instance.PlaySoundFXClip(hurtSoundEffect, transform, 0.5f);
    }



}
