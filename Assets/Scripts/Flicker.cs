// Code taken from here https://gist.github.com/sinbad/4a9ded6b00cf6063c36a4837b15df969

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour{
	
	public new Light light;

	public float minIntensity = 0f;

	public float maxIntensity = 1f;

	public int smoothing = 5;

	Queue<float> smoothQueue;
	float lastSum = 0;

	public void Reset(){
		smoothQueue.Clear();
		lastSum = 0;
	}

	void Start(){
		smoothQueue = new Queue<float>(smoothing);

		if(light == null) {
			light = GetComponent<Light>();
		}
        
    }

	void Update(){
		if(light == null){
			return;
		}
		// Dequeues the last value from the quque
		// resetubg the light to its base value
		while(smoothQueue.Count >= smoothing){
			lastSum -= smoothQueue.Dequeue();
		}
		// Adds new random value withing range to queue
		float newVal = Random.Range(minIntensity, maxIntensity);
		smoothQueue.Enqueue(newVal);
		// Adds the new value to the base light value
		lastSum += newVal;
		// Updates the light intensity
		light.intensity = lastSum/(float)smoothQueue.Count;
        
    }
}
