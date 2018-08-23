using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapScrolling : MonoBehaviour {

    [Range(1, 50)]
    [Header("Controllers")]
    public int panCount;

    [Header("Other Objects")]
    public GameObject panPrefab;
    public ScrollRect scrollRect;

    private GameObject[] instPans;
    private Vector2[] pansPos;
    private Vector2[] pansScale;

    [Range(1,100)]
    public int panOffset;
    [Range(0f, 5f)]
    public float scaleOffset;
    [Range(1f, 20f)]
    public float scaleSpeed;

    [Range(0f,20f)]
    public float snapSpeed;

    private RectTransform contentRect;
   
    private bool isScrolling;

    private Vector2 contentVector;
    private int selectedPanID;

	void Start () {

        instPans = new GameObject[panCount];
        pansPos = new Vector2[panCount];
        pansScale = new Vector2[panCount];

        contentRect = GetComponent<RectTransform>();

        for(int i =0;i<panCount; i++)
        {
            instPans[i] = Instantiate(panPrefab, transform, false) as GameObject;
            if (i == 0)
            {
                continue;
            }
            instPans[i].transform.localPosition = new Vector2(instPans[i - 1].transform.localPosition.x + 
                panPrefab.GetComponent<RectTransform>().sizeDelta.x + panOffset, instPans[i].transform.localPosition.y);
            pansPos[i] = -instPans[i].transform.localPosition;
        }
	}

    void FixedUpdate()
    {
        float nearestPos = float.MaxValue;
        for(int i=0; i < panCount; i++)
        {
            float distance = Mathf.Abs(contentRect.anchoredPosition.x - pansPos[i].x);
            if (distance < nearestPos)
            {
                nearestPos = distance;
                selectedPanID = i;
            }
            float scale = Mathf.Clamp(1 / (distance / panOffset) * scaleOffset, 0.5f, 1f);
            pansScale[i].x = Mathf.SmoothStep(instPans[i].transform.localScale.x, scale+0.3f, scaleSpeed * Time.fixedTime);
            pansScale[i].y = Mathf.SmoothStep(instPans[i].transform.localScale.y, scale+0.3f, scaleSpeed * Time.fixedTime);
            instPans[i].transform.localScale = pansScale[i];
        }
        float scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
        if(scrollVelocity <400 && !isScrolling)
        {
            scrollRect.inertia = false;
        }
        if (isScrolling || scrollVelocity >400) return;
        contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, pansPos[selectedPanID].x, snapSpeed * Time.fixedDeltaTime);
        contentRect.anchoredPosition = contentVector;
    }

    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
        if (scroll)
        {
            scrollRect.inertia = true;
        }
    }
	
}
