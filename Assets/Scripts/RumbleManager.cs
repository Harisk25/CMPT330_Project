using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleManager : MonoBehaviour
{
    public static RumbleManager instance;
    private Gamepad pad;
    private Coroutine stopRumble_AfterTimeCoroutine;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }
    public void RumblePulse(float lowFrequency, float highFrequency, float duration)
    {

        //get refernece to our Gamepad
        pad = Gamepad.current;

        if (pad != null)
        {
            //start rumble
            pad.SetMotorSpeeds(lowFrequency, highFrequency);

            //stop rumble after time
            stopRumble_AfterTimeCoroutine = StartCoroutine(StopRumble(duration, pad));
        }
    }

    private IEnumerator StopRumble(float duration, Gamepad pad)
    {
        float elaspedTime = 0f;
        while (elaspedTime < duration)
        {
            elaspedTime += Time.deltaTime;
            yield return null;
        }
        //once duration over
        pad.SetMotorSpeeds(0f, 0f);
    }


}
