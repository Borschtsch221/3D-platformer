using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

    public Image alphaImage;
    public float loadingTime = 0.5f;

     public Animator textAfterDeath;
    public CameraShake cameraShake;
    public CameraFollow cameraFollow;

    public PlayerCharacter player;

    public Text timerText;
	private float timer =0;

    void Start()
    {
        player.onChangeRenderer += OnPlayerChangeRenderer;
        pausePanelRect = pausePanel.GetComponent<RectTransform>();
        alphaImage.gameObject.SetActive(true);
        alphaImage.color = new Color(0,0,0,1);
        StartCoroutine(BeginPlay());
        
    }

    void OnPlayerChangeRenderer(){
        currentScore3 = false;
    }

    void Update(){
		timer+=Time.deltaTime;
        
       
            timer-=timer%0.01f;
            string s = timer.ToString();
            if(timer<10f){
                s = s.Insert(0, "0:0");
            }else{
                s = s.Insert(0, "0:");
            }
            //if(timer%0.01 == 0)
            timerText.text = s;
        

	}

    [Header("Pause menu")]
    public GameObject pausePanel;

    [Range(1f,2f)]
    public float scaleMultiplier;
    [Range(0.01f, 2f)]
    public float scalingTime;

    private bool paused = false;
    public void Pause(){
        StartCoroutine(SetPause());
        //SetPause();
    }

    private RectTransform pausePanelRect;

    IEnumerator SetPause(){
        Vector2 scale = pausePanelRect.localScale;
        if(paused){
            paused = false;
            Time.timeScale =1f;
            while(pausePanelRect.localScale.x>1){
                scale-=scale*scaleMultiplier*Time.unscaledDeltaTime/scalingTime;
                pausePanelRect.localScale = scale;
                yield return null;
            }
            pausePanel.SetActive(false);
        }
        else{
            paused = true;
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
            while(pausePanelRect.localScale.x <=scaleMultiplier){
                scale+=scale*scaleMultiplier*Time.unscaledDeltaTime/scalingTime;
                pausePanelRect.localScale = scale;
                yield return null;
            }
        }
    }

    public void Restart(){
        Application.LoadLevel(Application.loadedLevelName);
    }

    public void ToMainMenu(){
        alphaImage.gameObject.SetActive(true);
        pausePanel.SetActive(false);
        alphaImage.color = new Color(0,0,0,0);
        StartCoroutine(LoadWithBlackOut("MainMenu"));
    }

    private IEnumerator BeginPlay(){
        imageColor = alphaImage.color;
        while(alphaImage.color.a>0){
            imageColor.a -=Time.unscaledDeltaTime/loadingTime;
            alphaImage.color=imageColor;
            yield return null;
        }
        alphaImage.gameObject.SetActive(false);
    }
    



    private Color imageColor;

    private IEnumerator LoadWithBlackOut(string levelName)
    {
        imageColor = alphaImage.color;
        
        while (alphaImage.color.a < 1.0f)
        {            
            imageColor.a +=Time.unscaledDeltaTime/loadingTime;
            alphaImage.color = imageColor;
            yield return null;
        }
        Application.LoadLevel(levelName);
    }


    public int necessaryScore1 = 0;
    public int necessaryScore2 = 0;
    public int currentScore1 = 0;
    public int currentScore2 = 0;
    public bool currentScore3 = true;

    public void Win(string levelName){
        PlayerPrefs.SetInt("Level"+ (Application.loadedLevel+1).ToString() , 1);
 
        if(!PlayerPrefs.HasKey(Application.loadedLevelName+"_star1") && currentScore1>=necessaryScore1){
            PlayerPrefs.SetInt(Application.loadedLevelName+"_star1", 1);
        }

        if(!PlayerPrefs.HasKey(Application.loadedLevelName+"_star2") && currentScore2>=necessaryScore2){
            PlayerPrefs.SetInt(Application.loadedLevelName+"_star2", 1);
        }
        
        if(!PlayerPrefs.HasKey(Application.loadedLevelName+"_star3") && currentScore3){
            PlayerPrefs.SetInt(Application.loadedLevelName+"_star3", 1);
            Debug.Log("sdfs");
        }
        alphaImage.gameObject.SetActive(true);
        alphaImage.color = new Color(1,1,1,0);
        StartCoroutine( cameraFollow.Zoom());
        StartCoroutine(LoadWithBlackOut(levelName));

    }

    public void Death(string levelName){
        if (cameraShake)
        {
            StartCoroutine(cameraShake.Shake(.25f, .8f));
            if (textAfterDeath)
            {
                textAfterDeath.enabled = true;
            }
        }
        if (cameraFollow)
        {
            cameraFollow.follow = false;
            
        }
        alphaImage.gameObject.SetActive(true);
        alphaImage.color = new Color(0,0,0,0);
        StartCoroutine(LoadWithBlackOut(levelName));
    }

}
