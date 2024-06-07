using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class QuitButton : MonoBehaviour
{
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
        //If the player hovers over the quit button
        if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject)
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }
        //If the player clicks the quit button
        if (Input.GetMouseButtonDown(0)) // if mouse 1 is clicked on quit button quit the application
        {
            if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject)
            {
                Debug.Log("Button Clicked");
                OnClick.Invoke();
            }
        }
        //Shows the player what button they are hovering over
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

    public void Quit()
    {
        Application.Quit();
    }
}
