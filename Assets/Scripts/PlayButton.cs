using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayButton : MonoBehaviour
{
    public int sceneNumber = 0;
    public GameObject definedButton;
    public UnityEvent OnClick = new UnityEvent();
    public SpriteRenderer torch;
    public Flicker torchLight;

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
        //If player hover over play button it will be selected
        if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject)
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }
        //If player clicks on play button
        if (Input.GetMouseButtonDown(0)) // if mouse 1 clicks the play button start game
        {
            if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject)
            {
                Debug.Log("Button Clicked");
                OnClick.Invoke();
            }
        }
        //This helps the player see what is selected
        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            torch.enabled = true;
            torchLight.maxIntensity = 10;
        }
        else
        {
            torch.enabled = false;
            torchLight.maxIntensity = 0;
        }
        

    }

    public void Play()
    {
        SceneManager.LoadScene(sceneNumber); // load scene number allows to change to any scene wanted
    }

}
