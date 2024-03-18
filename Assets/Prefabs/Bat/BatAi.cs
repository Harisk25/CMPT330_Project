using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAi : MonoBehaviour{

    PlayerDetection playerDetection;
    private Animator bAnimator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D hitBox;
    private bool wasHit = false;
    private Transform target;
    private Transform lastDirection;
    private Vector2 moveDirection;
    private Rigidbody2D rb;

    [Header ("Stats")]
    public int health = 2;
    public float speed = 1.0f;
    public Transform start;
    public Transform end;


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
	for (int n = 0; n < 2; n++){
	    spriteRenderer.color = Color.clear;
	    yield return new WaitForSeconds(0.1f);
	    spriteRenderer.color = Color.white;
	    yield return new WaitForSeconds(0.1f);
	}
    }

    void Movement(){
	if(playerDetection.PlayerDetected && !wasHit){
	    target = playerDetection.Target.transform;
	    Vector3 direction= target.position - transform.position;
	    direction.y+=3;
	    direction = direction.normalized;
	    moveDirection = direction;

	}
	else if(!playerDetection.PlayerDetected && !wasHit){
	    
	    if(transform.position.x == end.position.x){
		lastDirection = start;
		target = start;
	    }
	    else if(transform.position.x == start.position.x){
		lastDirection = end;
		target = end;
	    }

	    Vector3 direction= (target.position - transform.position).normalized;
	    moveDirection = direction;

	}
	else if(!playerDetection.PlayerDetected && !wasHit){
	    
	    if(transform.position.x == end.position.x){
		lastDirection = start;
		target = start;
		wasHit = true;
	    }
	    else if(transform.position.x == start.position.x){
		lastDirection = end;
		target = end;
		wasHit = true;
	    }
	    Vector3 direction= (target.position - transform.position).normalized;
	    moveDirection = direction;

	}
	else if(wasHit){
	    target = lastDirection;
	    Vector3 direction= (target.position - transform.position).normalized;
	    moveDirection = direction;
	}

	rb.velocity = new Vector2(moveDirection.x, moveDirection.y)* speed;
	
		
    }

    void OnCollisionEnter2D(Collision2D other){
	
//	StartCoroutine(Flash());
	health--;
	Vector2 hitDir = (transform.position - playerDetection.Target.transform.position).normalized;
	rb.AddForce(hitDir*16.0f, ForceMode2D.Impulse);
	wasHit = true;

    }

    void EnemyDeath(){
	Destroy(gameObject);
    }
}
