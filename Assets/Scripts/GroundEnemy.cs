using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    [Header("PlayerCheck")]
    public Transform playerCheckPos;
    public Vector2 playerCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask playerLayer;
    bool inContactWithPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerCheck();
        GettingHit();
    }

    /*
     * GettingHit will reduce HP and trigger death if HP == 0
     */
    private void GettingHit()
    {
        if (inContactWithPlayer == true)
        {
            Debug.Log("Player Hitting Me! ");
        }
    }

    /*
     * PlayerCheck will see if the player is hitting the enemy.
     */
    private void PlayerCheck()
    {
        if (Physics2D.OverlapBox(playerCheckPos.position, playerCheckSize, 0, playerLayer))
        {
            inContactWithPlayer = true;
        }
        else
        {
            inContactWithPlayer = false;
        }
    }

    /*
     * OnDrawGizmosSelected draws our GroundCheck and WallCheck hit boxes so we can see how big they are.
     */
    private void OnDrawGizmosSelected()
    {
        //PlayerCheck
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(playerCheckPos.position, playerCheckSize);
    }
}
