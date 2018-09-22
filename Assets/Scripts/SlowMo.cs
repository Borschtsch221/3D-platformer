using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SlowMo : MonoBehaviour
{

    public float slowdownFactor;
    public PlayerCharacter player;

    private bool playerChangeState = false;

    private enum Action { none, jump, changeRenderer };

    public GameObject instancePanel;

    [SerializeField]
    private Action action;

    void Start()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (slowdownFactor != 0f)
            {
                Time.timeScale = slowdownFactor;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
            }

            
            switch (action)
            {
                case Action.changeRenderer:
                    player.onChangeRenderer += OnPlayerChangeState;
                    break;
                case Action.jump:
                    player.onJump += OnPlayerChangeState;
                    break;
            }
            if (instancePanel)
            {
                instancePanel.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && !playerChangeState)
        {
            StartCoroutine(ResetTimeScale());
            instancePanel.SetActive(false);
        }
    }

    IEnumerator ResetTimeScale()
    {
        while (Time.timeScale < 1)
        {
            Time.timeScale += Time.unscaledDeltaTime * slowdownFactor * 2;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            yield return null;
        }
    }

    void OnPlayerChangeState()
    {
        switch (action)
        {
            case Action.changeRenderer:
                player.onChangeRenderer -= OnPlayerChangeState;
                break;
            case Action.jump:
                player.onJump -= OnPlayerChangeState;
                break;
        }
        playerChangeState = true;
        StartCoroutine(ResetTimeScale());
        if (instancePanel)
        {
            instancePanel.SetActive(false);
        }
    }
}
