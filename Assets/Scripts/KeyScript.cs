using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
	[SerializeField] private AudioClip collectSoundClip;
	void OnTriggerEnter2D(Collider2D collision)
	{

		if (collision.gameObject.tag == "Player")
		{
			var player = collision.gameObject.GetComponent<PlayerMovement>();
			player.hasKey = true;
			SoundFXManager.instance.PlaySoundFXClip(collectSoundClip, transform, 0.7f);
			Destroy(gameObject);
		}


	}
}
