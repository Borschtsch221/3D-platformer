using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaController : MonoBehaviour {

    public GameObject alphaObj;
    private Image alphaImage;
    public int alphaA;

    void Start()
    {
        alphaImage = alphaObj.GetComponent<Image>();
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

}
