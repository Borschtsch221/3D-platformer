using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour {

    
    void OnCollisionEnter(Collision other)
    {
        PlayerCharacter player = other.gameObject.GetComponent<PlayerCharacter>();
        if (player)
        {
            player.Death();
        }
    }
}
