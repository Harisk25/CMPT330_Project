using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour
{
    public void Play()
    {
        Debug.Log("Play was pressed!");
        SceneManager.LoadScene("Tutorial");
    }
}
