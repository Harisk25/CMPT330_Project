using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible_Script : MonoBehaviour
{		
    // [Header ("UI")]
    // public UserInterface UI;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision){
	
    	if(collision.gameObject.tag == "Player"){
	    // UI.collectibleCount += 1;
	    // Play sound here
	    Destroy(gameObject);
	}
    
    }
}
