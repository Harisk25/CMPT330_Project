using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D collision)
	{

		if (collision.gameObject.tag == "Player")
		{
			var player = collision.gameObject.GetComponent<PlayerMovement>();
			player.hasKey = true;
			Destroy(gameObject);
		}


	}
}
