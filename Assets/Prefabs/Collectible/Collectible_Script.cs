using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible_Script : MonoBehaviour
<<<<<<< Updated upstream
{
    [Header("PlayerCharacter")]
    public PlayerMovement player;
=======
{		
    [SerializeField] private AudioClip collectSoundClip;

>>>>>>> Stashed changes
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision){
	
    	if(collision.gameObject.tag == "Player"){
<<<<<<< Updated upstream
            player.scoreCount += 100;
	        // Play sound here
	        Destroy(gameObject);
	    }
=======
	    var player = collision.gameObject.GetComponent<PlayerMovement>();
	    if(player.playerHP < 4){
	    	player.playerHP += 1;
	    }
	    SoundFXManager.instance.PlaySoundFXClip(collectSoundClip, transform, 0.7f);
	    Destroy(gameObject);
	}
>>>>>>> Stashed changes
    
    }
}
