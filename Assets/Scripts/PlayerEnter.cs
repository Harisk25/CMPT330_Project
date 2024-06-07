using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnter : MonoBehaviour
{
    public Boss boss;
	void OnTriggerEnter2D(Collider2D collision) // check if player collides with door object
	{

		if (collision.gameObject.tag == "Player")
		{
			boss.playerEnter = true;
			Destroy(gameObject);
		}


	}
}
