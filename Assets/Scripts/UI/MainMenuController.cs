using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {

	public GameObject UILevel1;
	public GameObject ScrollView;
	public GameObject Settings;


	public void ToScrollView(){
		UILevel1.SetActive(false);
		ScrollView.SetActive(true);
	}

	public void ToUILevel1(){
		ScrollView.SetActive(false);
		Settings.SetActive(false);
		UILevel1.SetActive(true);
	}

	public void ToSettings(){
		UILevel1.SetActive(false);
		Settings.SetActive(true);
	}

	public void Exit(){
		Application.Quit();
	}
}
