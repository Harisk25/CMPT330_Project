using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
	public int sceneNumber = 0;
	void OnTriggerEnter2D(Collider2D collision)
	{

		if (collision.gameObject.tag == "Player")
		{
			var player = collision.gameObject.GetComponent<PlayerMovement>();
			if(player.hasKey == true)
            {
				SceneManager.LoadScene(sceneNumber);
			}
		}


	}
}
