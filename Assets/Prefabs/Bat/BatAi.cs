using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAi : MonoBehaviour{

    PlayerDetection playerDetection;
    private Animator bAnimator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D hitBox;
    private bool wasHit = false;
    private bool gotHit = false;
    private Transform target;
    private Transform lastDirection;
    private Vector2 moveDirection;
    private Rigidbody2D rb;


    [Header ("Stats")]
    public int health = 2;
    public float speed = 1.0f;
    public Transform start;
    public Transform end;

    [SerializeField] private AudioClip damageSoundClip;
    [SerializeField] private AudioClip damage2SoundClip;


    void Awake(){
        bAnimator = GetComponent<Animator>();
	playerDetection = GetComponent<PlayerDetection>();
	spriteRenderer = GetComponent<SpriteRenderer>();
	rb = GetComponent<Rigidbody2D>();
	hitBox = GetComponent<BoxCollider2D>();
	target = end;
	lastDirection = end;
    }

    void Update(){
	if(health < 1){
	    EnemyDeath();
	}
	Movement();
    }

    private IEnumerator Flash(){
	// Causes sprite to flash after being hit
	for (int n = 0; n < 2; n++){
	    spriteRenderer.color = Color.clear;
	    yield return new WaitForSeconds(0.1f);
	    spriteRenderer.color = Color.white;
	    yield return new WaitForSeconds(0.1f);
	}
    }

    private IEnumerator GotHit(){
	    // Causes a pause in movement for a breif time
	rb.velocity = new Vector2(0,0);
	gotHit = true;
	yield return new WaitForSeconds(0.5f);
	gotHit = false;

    }


    void Movement(){
	
	var outOfRange = PlayerOutOfRange();
	
	// Movement between two transform points if it detects the player it will head
	// to the player instead and it it was hit it will be starionary for a moment
	// before contining. If the player leave the detection zone it will return 
	// to the movement route between the two transform points
	if(playerDetection.PlayerDetected && !wasHit && !outOfRange){
	    target = playerDetection.Target.transform;
	    Vector3 direction= target.position - transform.position;
	    direction.y+=5;
	    direction = direction.normalized;
	    moveDirection = direction;

	}
	else if(playerDetection.PlayerDetected && !wasHit && outOfRange){
	    
	    if(transform.position.x >= end.position.x){
		lastDirection = start;
		target = start;
	    }
	    else if(transform.position.x <= start.position.x){
		lastDirection = end;
		target = end;
	    }

	    Vector3 direction= (target.position - transform.position).normalized;
	    moveDirection = direction;

	}

	else if(!playerDetection.PlayerDetected && !wasHit){
	    
	    if(transform.position.x >= end.position.x){
		lastDirection = start;
		target = start;
	    }
	    else if(transform.position.x <= start.position.x){
		lastDirection = end;
		target = end;
	    }

	    Vector3 direction= (target.position - transform.position).normalized;
	    moveDirection = direction;

	}
	

	else if(wasHit){
	    target = lastDirection;
	    if(target.transform.position.x >= end.position.x || target.transform.position.x <= start.position.x){
		wasHit = false;
	    }
	    Vector3 direction= (target.position - transform.position).normalized;
	    moveDirection = direction;
		

	}
	if(!gotHit){
	    rb.velocity = new Vector2(moveDirection.x, moveDirection.y)* speed;
	}
	
		
    }

    void OnTriggerEnter2D(Collider2D other){
	// Plays hit sounds, causes animation to flash for a short time and stops movement
	// when hit with player sword
	SoundFXManager.instance.PlaySoundFXClip(damageSoundClip, transform, 0.4f);
	SoundFXManager.instance.PlaySoundFXClip(damage2SoundClip, transform, 0.7f);
	StartCoroutine(Flash());
	health--;
	StartCoroutine(GotHit());
	wasHit = true;

    }

    void EnemyDeath(){
	Destroy(gameObject);
    }

    bool PlayerOutOfRange(){
	if(playerDetection.PlayerDetected){
	   if(playerDetection.Target.transform.position.x < start.position.x || playerDetection.Target.transform.position.x > end.position.x){
		return true;
	    }
	    return false;
	}
	return false;
    }
}
