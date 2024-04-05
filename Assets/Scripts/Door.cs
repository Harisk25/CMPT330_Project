using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
	public int sceneNumber = 0;
	void OnTriggerEnter2D(Collider2D collision) // check if player collides with door object
	{

		if (collision.gameObject.tag == "Player")
		{
			var player = collision.gameObject.GetComponent<PlayerMovement>();
			if(player.hasKey == true) // if player has key, change scenes
            {
				SceneManager.LoadScene(sceneNumber);
			}
		}


	}
}
