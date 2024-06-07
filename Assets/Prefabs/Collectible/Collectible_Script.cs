using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible_Script : MonoBehaviour
{		
    [SerializeField] private AudioClip collectSoundClip;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision){
	// If the player hitbox comes in contact with it will heal the player,
	// play a sound effect and remove the collectible object
    	if(collision.gameObject.tag == "Player"){
	    var player = collision.gameObject.GetComponent<PlayerMovement>();
            player.scoreCount += 100;
	    if(player.playerHP < 4){
	    	player.playerHP += 1;
	    }
	    SoundFXManager.instance.PlaySoundFXClip(collectSoundClip, transform, 0.3f);
	    Destroy(gameObject);
	}

    
    }
}
