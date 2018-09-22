using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ventilator : MonoBehaviour {

	private PlayerCharacter player = null;
	private Rigidbody playerRigidbody = null;
	//public float force = 30f;
	public Vector2 force;

	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			player = other.GetComponent<PlayerCharacter>();
			player.Controlled = false;
			playerRigidbody = player.GetComponent<Rigidbody>();
		}
	}
	void OnTriggerStay(Collider other){
		if(other.tag == "Player"){
			playerRigidbody.AddForce(Vector3.up * force.y+Vector3.left*force.x);
		}
	}

	void OnTriggerExit(Collider other){
		if(other.tag=="Player"){
			player.Controlled = true;
		}
	}
}
