using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

	public Text levelText;
	public int unlocked;
	public GameObject star1;
	public GameObject star2;
	public GameObject star3;

	public void Log(){
		Debug.Log("button is pressed");
	}
	 
}
