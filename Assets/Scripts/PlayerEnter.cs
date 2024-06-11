using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnter : MonoBehaviour
{
    public Boss boss;
	public GameObject wall;
	public new GameObject light;
	public AudioSource music1;
	public AudioClip music2;
	public GameObject bossUI;


	void OnTriggerEnter2D(Collider2D collision) // check if player collides with door object
	{

		if (collision.gameObject.tag == "Player")
		{
			boss.playerEnter = true;
			wall.SetActive(true);
			Destroy(light);
			bossUI.SetActive(true);
			music1.Stop();
			music1.clip = music2;
			music1.Play();
			Destroy(gameObject);
		}

	}
}
