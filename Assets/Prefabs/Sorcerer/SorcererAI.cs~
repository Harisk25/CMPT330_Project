using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorcererAI : MonoBehaviour{

    PlayerDetection playerDetection;
    private Animator sAnimator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D hitBox;
    
    [Header ("Stats")]
    public int health = 3;
    public float invincibleTime = 0.7f;
    public GameObject Fireball;
    public Transform fireballSpawn1;
    public Transform fireballSpawn2;
    [SerializeField] private AudioClip attackSoundClip;
    [SerializeField] private AudioClip hitSoundClip;
    [SerializeField] private AudioClip deathSoundClip;



    void Awake(){
	sAnimator = GetComponent<Animator>();
	playerDetection = GetComponent<PlayerDetection>();
	spriteRenderer = GetComponent<SpriteRenderer>();
	hitBox = GetComponent<BoxCollider2D>();

    }

    void Update(){
	setAnimation();
	

    }

    void setAnimation(){
    	if(sAnimator != null){
	    

	    if(playerDetection.PlayerDetected){
		if(playerDetection.DirectionToTarget.x < 0){
		    spriteRenderer.flipX = true;
		}
		else{
		    spriteRenderer.flipX = false;
		}
		if(sAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle")){
		    sAnimator.SetTrigger("TrAttack");
		}
	    }
	    

	}
    }
    
    void OnCollisionEnter2D(Collision2D other){
	sAnimator.SetTrigger("TrHit");
	health--;
	if(health == 0){
	    sAnimator.SetTrigger("TrDeath");
	}
    }

    void EnemyDeath(){
	Destroy(gameObject);
    }

    void SpawnProjectile(){
	SoundFXManager.instance.PlaySoundFXClip(attackSoundClip, transform, 0.6f);
	if(spriteRenderer.flipX){

	    var fireball = Instantiate(Fireball, fireballSpawn2.position, fireballSpawn2.rotation);
	}
	else{
	    var fireball = Instantiate(Fireball, fireballSpawn1.position, fireballSpawn1.rotation);

	}
    }

    public void PlayHitSound(){
    	SoundFXManager.instance.PlaySoundFXClip(hitSoundClip, transform, 0.5f);
    }
    public void PlayDeathSound(){
    	SoundFXManager.instance.PlaySoundFXClip(deathSoundClip, transform, 0.8f);
    }
}
