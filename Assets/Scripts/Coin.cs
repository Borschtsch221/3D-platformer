using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

	private enum TypeOfCoin{ type1, type2 };
	[SerializeField] 
	private TypeOfCoin typeOfCoin;
	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			switch (typeOfCoin){
				case TypeOfCoin.type1:
					GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>().currentScore1++;
					break;
				case TypeOfCoin.type2: 
					GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>().currentScore2++;
					break;				
			}
			Destroy(gameObject);			
		}
	}
}
