using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

    //public GameObject alphaObj;
    public Image alphaImage;
    public float loadingTime = 0.5f;
    private float realLoadingTime;

     public Animator textAfterDeath;
    public CameraShake cameraShake;
    public CameraFollow cameraFollow;

    void Start()
    {
        //alphaImage = alphaObj.GetComponent<Image>();
        realLoadingTime = loadingTime * 2;
    }


    public int score = 0;



    private Color imageColor;

    private IEnumerator LoadWithDodge(string levelName)
    {
        
        while (alphaImage.color.a < 1.0f)
        {
            imageColor = alphaImage.color;
            imageColor.a +=realLoadingTime*Time.deltaTime;
            alphaImage.color = imageColor;
            yield return null;
        }
        Application.LoadLevel(levelName);
    }

    public void Win(string levelName){
        PlayerPrefs.SetInt("Level"+ (Application.loadedLevel+1).ToString() , 1);
        PlayerPrefs.SetInt(Application.loadedLevelName+"_score", score);
        alphaImage.color = new Color(1,1,1,0);
        StartCoroutine( cameraFollow.Zoom());
        StartCoroutine(LoadWithDodge(levelName));

    }

    public void Death(string levelName){
        if (cameraShake)
        {
            StartCoroutine(cameraShake.Shake(.15f, .4f));
            if (textAfterDeath)
            {
                textAfterDeath.enabled = true;
            }
        }
        if (cameraFollow)
        {
            cameraFollow.follow = false;
            
        }
        alphaImage.color = new Color(0,0,0,0);
        StartCoroutine(LoadWithDodge(levelName));
    }

}
