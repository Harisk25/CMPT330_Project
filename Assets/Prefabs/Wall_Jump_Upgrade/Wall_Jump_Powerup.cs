using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Jump_Powerup : MonoBehaviour
{  
    [Header ("PlayerCharacter")]
    public PlayerMovement player;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision){
	
    	if(collision.gameObject.tag == "Player"){
	    player.wallPowerupActive = true;
	    // Play sound here
	    Destroy(gameObject);
	}
    
    }
}
