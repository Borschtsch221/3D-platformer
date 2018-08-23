using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

    public GameObject alphaObj;
    private Image alphaImage;
    public int alphaA;
    public float loadingTime = 0.5f;
    private float realLoadingTime;

    void Start()
    {
        alphaImage = alphaObj.GetComponent<Image>();
        realLoadingTime = loadingTime * 2;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            if (alphaA == 0)
            {
                alphaImage.color = new Color(alphaImage.color.r, alphaImage.color.g, alphaImage.color.b, alphaImage.color.a + 0.5f * Time.deltaTime);
                if (alphaImage.color.a >= 1.0f)
                {
                    alphaA = 1;
                }
            }
            if(alphaA==1)
            {
                alphaImage.color = new Color(alphaImage.color.r, alphaImage.color.g, alphaImage.color.b, alphaImage.color.a - 0.5f * Time.deltaTime);
                if (alphaImage.color.a <= 0f)
                {
                    alphaA = 0;
                }
            }
        }
    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadWithDodge(levelName));
    }

    public void LoadLevel(int levelNumber)
    {

    }

    private IEnumerator LoadWithDodge(string levelName)
    {
        while (alphaImage.color.a < 1.0f)
        {
            alphaImage.color = new Color(alphaImage.color.r, alphaImage.color.g, alphaImage.color.b, alphaImage.color.a + realLoadingTime * Time.deltaTime);
            yield return null;
        }
        Application.LoadLevel(levelName);
    }

}
