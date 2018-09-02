using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour {

	[System.Serializable]
	public class Level{
		public string levelText;
		public int unlocked;
		public bool isInteratable;
		
		public Button.ButtonClickedEvent onClickEvent;

	}

	public List<Level> levelList;
	public GameObject button;
}
