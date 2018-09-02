using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class time : MonoBehaviour {


	public int score = 10000;

	// Use this for initialization
	void Start () {
		StartCoroutine(loadLevel());
	}
	
	public IEnumerator loadLevel(){
		yield return new WaitForSeconds(2);
		PlayerPrefs.SetInt("Level2", 1);
		PlayerPrefs.SetInt("Level1_score", 5001);
		Application.LoadLevel("MainMenu");
	}
}
