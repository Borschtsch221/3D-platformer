using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCharacter : MonoBehaviour
{

    public delegate void PlayerChangeState();
    public event PlayerChangeState onJump;
    public event PlayerChangeState onChangeRenderer;




    public LevelController levelController;

    private Rigidbody rigidbody;
    private Renderer renderer;

    public Material[] materials;
    public Atributes.Color color;

    [Range(1f, 100f)]
    public float speed = 5f;
    

    private bool isDead = false;
    private bool isWinner = false;

    private Vector3 newVelocity;

    private bool controlled = true;


    private enum TypeOfController { none, touch, keyboard };
    [SerializeField]
    TypeOfController typeOfController;

    private delegate void Controller();
    private Controller controller;


    void Start()
    {

        rigidbody = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();

        switch (typeOfController)
        {
            case TypeOfController.touch: controller += TouchController; break;
            case TypeOfController.keyboard: controller += KeyboardController; break;
            case TypeOfController.none: controlled = false; break;
        }
    }



    void FixedUpdate()
    {

        if (controlled)
        {
            newVelocity = rigidbody.velocity;
            newVelocity.x = speed;
            controller();
            rigidbody.velocity = newVelocity;
        }

    }

    void TouchController()
    {

        foreach (var touch in Input.touches)
        {
            if (touch.position.x < Screen.width / 2)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    if (color == Atributes.Color.purple)
                    {
                        renderer.material = materials[1];
                        color = Atributes.Color.yellow;
                    }
                    else
                    {
                        renderer.material = materials[0];
                        color = Atributes.Color.purple;
                    }
                    if (onChangeRenderer != null)
                    {
                        onChangeRenderer();
                    }
                }
            }
            else
            {
                if ((touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) && currentJumps < maxJumps && curJumpDurability < jumpDurability)
                {
                    if (onJump != null)
                    {
                        onJump();
                    }
                    curJumpDurability += Time.fixedDeltaTime;
                    float jf = startForce - (curJumpDurability * curJumpDurability) / jumpForce;
                    if (jumpForce >= 0)
                        newVelocity.y = jf;
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    currentJumps++;
                    curJumpDurability = 0;
                }

            }
        }
        rigidbody.AddForce(Vector3.down * fallingSpeed);

    }



    [Header("Jump")]
    public float jumpDurability = 1f;
    private float curJumpDurability = 0;

    [Range(0.01f, 2f)]
    public float jumpForce = 5f;

    [Range(1, 5)]
    public int maxJumps = 2;
    private int currentJumps = 0;

    [Range(3f, 15f)]
    public float startForce = 5;

    [Range(0f, 50f)]
    public float fallingSpeed = 15f;

    void KeyboardController()
    {
        if (Input.GetKey(KeyCode.Space) && currentJumps < maxJumps && curJumpDurability < jumpDurability)
        {
            if (onJump != null)
            {
                onJump();
            }
            curJumpDurability += Time.fixedDeltaTime;
            float jf = startForce - (curJumpDurability * curJumpDurability) / jumpForce;
            if (jumpForce >= 0)
                newVelocity.y = jf;
        }
        rigidbody.AddForce(Vector3.down * fallingSpeed);

        if (Input.GetKeyUp(KeyCode.Space))
        {
            curJumpDurability = 0;
            currentJumps++;
        }


        if (Input.GetKeyDown(KeyCode.X))
        {
            if (color == Atributes.Color.purple)
            {
                renderer.material = materials[1];
                color = Atributes.Color.yellow;
            }
            else
            {
                renderer.material = materials[0];
                color = Atributes.Color.purple;
            }
            if (onChangeRenderer != null)
            {
                onChangeRenderer();
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        currentJumps = 0;

        PlatformScript platform = other.gameObject.GetComponent<PlatformScript>();
        if (platform)
        {
            if (platform.color != color)
            {
                Death();
            }
        }
    }

    void OnCollisionStay(Collision other)
    {
        currentJumps = 0;

        PlatformScript platform = other.gameObject.GetComponent<PlatformScript>();
        if (platform)
        {
            if (platform.color != color)
            {
                Death();
            }
        }
    }

    public void Death()
    {
        if (isDead || isWinner)
        {
            return;
        }
        isDead = true;
        renderer.enabled = false;
        levelController.Death(Application.loadedLevelName);
    }

    public void Win()
    {
        if (isDead)
        {
            return;
        }
        isWinner = true;
        controlled = false;
        levelController.Win("MainMenu");
    }
}
