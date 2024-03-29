using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrowNPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public string dialogue;
    private int index;
    private int playerEntered = 1;

    public float wordSpeed;
    public bool playerIsClose;
    

    // Update is called once per frame
    void Update()
    {
        if(playerEntered == 1 && playerIsClose == true)
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

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        foreach(char letter in dialogue.ToCharArray())
        {
            if(playerIsClose == false)
            {
                break;
            }
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
            playerEntered = 1;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
        }
    }


}
