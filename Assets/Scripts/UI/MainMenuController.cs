using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	public GameObject UILevel1;
	public GameObject ScrollView;
	public GameObject Settings;


	private RectTransform UILevel1Rect;
	private RectTransform ScrollViewRect;
	private RectTransform SettingsRect;

	private RectTransform hideRect = null;
	public RectTransform showRect = null;

	private int hideRectIndex = 0;
	private int showRectIndex = 0;


	void Start(){
		if(Time.timeScale!=1){
			Time.timeScale = 1f;
		}
		UILevel1Rect = UILevel1.GetComponent<RectTransform>();
		ScrollViewRect = ScrollView.GetComponent<RectTransform>();
		SettingsRect = Settings.GetComponent<RectTransform>();

		ScrollViewRect.anchoredPosition = new Vector2(ScrollViewRect.anchoredPosition.x, -ScrollViewRect.rect.height);
		SettingsRect.anchoredPosition = new Vector2 (SettingsRect.anchoredPosition.x, -SettingsRect.rect.height);
	}




	private Vector2 tempPos;

	IEnumerator HideAndShow(){
		tempPos = hideRect.anchoredPosition;
		
		float tempY = 0;
		while(tempY<hideRect.rect.height){
			tempPos.y  -= hideRect.rect.height *speed *Time.fixedDeltaTime;
			//tempPos.y = Mathf.Clamp(tempPos.y, 0, -1080);
			tempY+=hideRect.rect.height *speed *Time.fixedDeltaTime;
			hideRect.anchoredPosition = tempPos;
			yield return null;
		}
		switch(hideRectIndex){
			case 0: UILevel1.SetActive(false); break;
			case 1: ScrollView.SetActive(false); break;
			case 2: Settings.SetActive(false); break;
		}
		tempPos = showRect.anchoredPosition;
		tempY =0;
		switch(showRectIndex){
			case 0: UILevel1.SetActive(true); showRectIndex = -1; break;
			case 1: ScrollView.SetActive(true);showRectIndex = -1; break;
			case 2: Settings.SetActive(true);showRectIndex = -1; break;
		}
		while(tempY<showRect.rect.height){
			tempPos.y+=showRect.rect.height*speed*Time.fixedDeltaTime;
			tempY+=showRect.rect.height *speed *Time.fixedDeltaTime;
			//tempPos.y = Mathf.Clamp(tempPos.y, 0, -1080);
			showRect.anchoredPosition = tempPos;
			yield return null;
		}
	}


	public void ToUILevel1(){
		if(ScrollView.active==false){
			//Settings.SetActive(false);
			hideRect = SettingsRect;	
			hideRectIndex = 2;		
		}
		else{
			//ScrollView.SetActive(false);
			hideRect = ScrollViewRect;	
			hideRectIndex = 1;			
		}
		showRect = UILevel1Rect;
		showRectIndex =0;
		//UILevel1.SetActive(true);
		StartCoroutine(HideAndShow());
	}

	Vector2 panelPos;
	public float speed = 6000;
	

	public void ToSettings(){
		// UILevel1.SetActive(false);
		// Settings.SetActive(true);		
		hideRect = UILevel1Rect;
		hideRectIndex = 0;
		showRect = SettingsRect;
		showRectIndex = 2;
		StartCoroutine(HideAndShow());
	}
	public void ToScrollView(){
		// UILevel1.SetActive(false);
		// ScrollView.SetActive(true);	
		hideRect = UILevel1Rect;
		hideRectIndex = 0;
		showRect = ScrollViewRect;
		showRectIndex =1;
		StartCoroutine(HideAndShow());
	}

	public void Exit(){
		Application.Quit();
	}

	public void ResetGameProgress(){
		PlayerPrefs.DeleteAll();
        Application.LoadLevel(Application.loadedLevelName);
	}
}
