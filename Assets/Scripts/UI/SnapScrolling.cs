using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapScrolling : MonoBehaviour
{

    [System.Serializable]
    public class Level
    {
        public string levelText;
        public int unlocked;
        public bool isInteratable;
    }

    public List<Level> levelList;
    //public GameObject button;







    [Range(1, 50)]
    [Header("Controllers")]
    private int panCount;

    [Header("Other Objects")]
    public GameObject panPrefab;
    public ScrollRect scrollRect;



    private GameObject[] instPans;
    private Vector2[] pansPos;
    private Vector2[] pansScale;
    private Color[] pansColor;


    [Range(1, 100)]
    public int panOffset;
    [Range(0f, 5f)]
    public float scaleOffset;
    [Range(1f, 20f)]
    public float scaleSpeed;

    [Range(0f, 20f)]
    public float snapSpeed;

    private RectTransform contentRect;

    private bool isScrolling;

    private Vector2 contentVector;
    private int selectedPanID;

    [Range(0f, 1f)]
    public float colorOffset;
    [Range(1f, 20f)]
    public float colorSpeed;


    
    void Start()
    {
        //DeleteAll();
        FillList();
    }

    void FixedUpdate()
    {
        if (contentRect.anchoredPosition.x >= pansPos[0].x && !isScrolling || contentRect.anchoredPosition.x <= pansPos[pansPos.Length - 1].x)
        {
            isScrolling = false;
            scrollRect.inertia = false;
        }
        float nearestPos = float.MaxValue;
        for (int i = 0; i < panCount; i++)
        {
            float distance = Mathf.Abs(contentRect.anchoredPosition.x - pansPos[i].x);

            if (distance < nearestPos)
            {
                nearestPos = distance;
                selectedPanID = i;
            }

            float scale = Mathf.Clamp(1 / (distance / panOffset) * scaleOffset, 0.85f, 1f);

            pansScale[i].x = Mathf.SmoothStep(instPans[i].transform.localScale.x, scale, scaleSpeed * Time.fixedDeltaTime);
            pansScale[i].y = Mathf.SmoothStep(instPans[i].transform.localScale.y, scale, scaleSpeed * Time.fixedDeltaTime);
            instPans[i].transform.localScale = pansScale[i];

            float alphaChange = Mathf.Clamp(1 / (distance / panOffset) * colorOffset, 0f, 1f);
            pansColor[i].a = Mathf.SmoothStep(instPans[i].GetComponent<Image>().color.a, alphaChange, colorSpeed * Time.fixedDeltaTime);
            instPans[i].GetComponent<Image>().color = pansColor[i];

        }



        float scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
        if (scrollVelocity < 400 && !isScrolling)
        {
            scrollRect.inertia = false;
        }
        if (isScrolling || scrollVelocity > 400) return;
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

    void FillList()
    {
        panCount = levelList.Count;
        instPans = new GameObject[panCount];
        pansPos = new Vector2[panCount];
        pansScale = new Vector2[panCount];
        pansColor = new Color[panCount];
        contentRect = GetComponent<RectTransform>();

        int j = 0;
        foreach (var level in levelList)
        {

            instPans[j] = Instantiate(panPrefab, transform, false) as GameObject;
            LevelButton button = instPans[j].GetComponent<LevelButton>();
            button.levelText.text = level.levelText;
            if (PlayerPrefs.GetInt("Level" + button.levelText.text) == 1)
            {
                level.unlocked = 1;
                level.isInteratable = true;
                if(int.TryParse(button.levelText.text,out selectedPanID)){
                    selectedPanID--;
                }
                //Debug.Log(selectedPanID);

            }
            button.unlocked = level.unlocked;
            Button buttonComponent = button.GetComponent<Button>();
            buttonComponent.interactable = level.isInteratable;
            buttonComponent.onClick.AddListener(() => LoadLevel("Level" + button.levelText.text));

            if (PlayerPrefs.GetInt("Level" + button.levelText.text + "_score") > 0)
            {
                button.star1.SetActive(true);
            }
            if (PlayerPrefs.GetInt("Level" + button.levelText.text + "_score") > 5000)
            {
                button.star2.SetActive(true);
            }
            if (PlayerPrefs.GetInt("Level" + button.levelText.text + "_score") > 9999)
            {
                button.star3.SetActive(true);
            }


            pansColor[j] = instPans[j].GetComponent<Image>().color;
            if (j == 0)
            {
                j++;
                continue;
            }
            instPans[j].transform.localPosition = new Vector2(instPans[j - 1].transform.localPosition.x +
                panPrefab.GetComponent<RectTransform>().sizeDelta.x, instPans[j].transform.localPosition.y);
            pansPos[j] = -instPans[j].transform.localPosition;



            j++;
        }

        contentVector.x = pansPos[selectedPanID].x;
        contentRect.anchoredPosition = contentVector;

        SaveAll();
    }

    void SaveAll()
    {
        if (PlayerPrefs.HasKey("Level1"))
        {
            return;
        }

        GameObject[] allButtons = GameObject.FindGameObjectsWithTag("LevelButton");
        foreach (var buttons in allButtons)
        {
            LevelButton button = buttons.GetComponent<LevelButton>();
            PlayerPrefs.SetInt("Level" + button.levelText.text, button.unlocked);
        }
    }

    void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    void LoadLevel(string levelName)
    {
        Debug.Log(levelName);
        Application.LoadLevel(levelName);
    }

}
