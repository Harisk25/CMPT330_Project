using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
    [Header("PlayerCheck")]
    public Transform playerCheckPos;
    public Vector2 playerCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask playerHitBoxLayer;
    [Header("Pick scene to load")]
    public int sceneNumber = 0;


    void Update()
    {
        PlayerCheck();

    }
    private void PlayerCheck() // once player collides with object change scenes
    {
        if (Physics2D.OverlapBox(playerCheckPos.position, playerCheckSize, 0, playerHitBoxLayer))
        {
            SceneManager.LoadScene(sceneNumber);
        }
    }
    private void OnDrawGizmosSelected()
    {

        //PlayerCheck
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(playerCheckPos.position, playerCheckSize);
        
    }
}
