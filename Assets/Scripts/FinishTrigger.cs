using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour {




    void Start()
    {
        GetComponent<ParticleSystem>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("finish");
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player)
        {
            
            player.Win();
        }
    }
}
