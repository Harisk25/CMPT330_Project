using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public int sceneNumber = 0;
    public GameObject definedButton;
    public UnityEvent OnClick = new UnityEvent();

    // Use this for initialization
    void Start()
    {
        definedButton = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;

        if (Input.GetMouseButtonDown(0)) // if mouse 1 clicks the play button start game
        {
            if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject)
            {
                Debug.Log("Button Clicked");
                OnClick.Invoke();
            }
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(sceneNumber); // load scene number allows to change to any scene wanted
    }
}
