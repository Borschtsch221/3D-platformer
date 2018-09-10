using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SlowMo : MonoBehaviour {

	public float slowdownFactor;
	public PlayerCharacter player;
	
	private bool resetTimeScale = false;

	private enum Action{none, jump, changeRenderer};

	public GameObject instancePanel;
	
	[SerializeField]
	private Action action;

	void Start(){
		Time.timeScale = 1f;
		Time.fixedDeltaTime = 0.02f;
	}
	 
	void OnTriggerEnter(Collider other){
		if(other.tag=="Player"){
			Time.timeScale = slowdownFactor;
			Time.fixedDeltaTime = Time.timeScale*0.02f;
			
			switch(action){
				case Action.changeRenderer:
					player.onChangeRenderer += OnPlayerChangeState;
					break;
				case Action.jump:
					player.onJump += OnPlayerChangeState;
					break;

			}
			if(instancePanel){
				instancePanel.SetActive(true);
			}
				
			
			
		}
	}

	void FixedUpdate(){
		if(resetTimeScale){
			Time.timeScale += Time.unscaledDeltaTime*slowdownFactor*2;
			Time.timeScale=Mathf.Clamp(Time.timeScale, 0f,1f);
			Time.fixedDeltaTime = Time.timeScale*0.02f;
		}
		if(Time.timeScale>=1){
			resetTimeScale=false;
		}
	}
	void OnPlayerChangeState(){
		resetTimeScale = true;
		Debug.Log("onplayer");
		switch(action){
			case Action.changeRenderer:
				player.onChangeRenderer -= OnPlayerChangeState;
				break;
			case Action.jump:
				player.onJump -= OnPlayerChangeState;
				break;

		}
		if(instancePanel){
			instancePanel.SetActive(false);
		}
			
	}

	
}
