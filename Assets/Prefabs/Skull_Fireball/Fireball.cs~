using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private Vector2 direction;
    private Animator sAnimator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private GameObject player;
    private float speed = 20.0f;

    [SerializeField] private AudioClip hitSoundClip;


    void Awake(){
	sAnimator = GetComponent<Animator>();
	spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
	player = GameObject.FindGameObjectWithTag("Player");
	Vector2 direction = player.transform.position - transform.position;
	rb.velocity = new Vector2(direction.x, direction.y+3).normalized * speed;
	
	if(direction.x < 0){
	    spriteRenderer.flipX = true;
	}
	else{
	    spriteRenderer.flipX = false;
	}
    }

    void Update(){
        
    }

    void OnTriggerEnter2D(Collider2D other){
	var parent = other.GetComponentInParent<Transform>();

	if(parent.CompareTag("Player")){
	    rb.velocity = new Vector2(0, 0);
	    sAnimator.SetTrigger("TrObjectHit");
	}
	else if(parent.CompareTag("Solid")){
	    rb.velocity = new Vector2(0, 0);
	    sAnimator.SetTrigger("TrObjectHit");
	}

    }

    void ObjectHit(){
	SoundFXManager.instance.PlaySoundFXClip(hitSoundClip, transform, 0.7f);
	Destroy(gameObject);
    }
}
