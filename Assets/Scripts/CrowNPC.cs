using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrowNPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public string dialogue;
    private int playerEntered = 1;

    public float wordSpeed;
    public bool playerIsClose;
    

    // Update is called once per frame
    void Update()
    {
        if(playerEntered == 1 && playerIsClose == true) // Player in range
        {
            if (dialoguePanel.activeInHierarchy)
            {
                zeroText();
            }
            else
            {
                playerEntered = 2;
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }
    }

    public void zeroText() // deletes text
    {
        dialogueText.text = "";
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing() // will simulate typing instead of just showing a full sentence 
    {
        foreach(char letter in dialogue.ToCharArray())
        {
            if(playerIsClose == false)
            {
                break;
            }
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed); // wait before adding next character
        }
    }

    private void OnTriggerEnter2D(Collider2D other) // if player enters area, start text
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
            playerEntered = 1; // marked as entered so that the text can type
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // player left area, stop typing
        {
            playerIsClose = false;
            zeroText();
        }
    }


}
