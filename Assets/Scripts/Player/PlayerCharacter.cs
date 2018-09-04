using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCharacter : MonoBehaviour
{


    public LevelController levelController;

    private Rigidbody rigidbody;
    private Renderer renderer;

    public Material[] materials;

    public float speed = 5f;
    public float jumpForce = 5f;
    public int maxJumps = 2;
    private int currentJumps = 0;


    public Atributes.Color color;


    public float jumpDurability = 1f;
    private float curJumpDurability;





    private bool isDead = false;
    private bool isWinner = false;

    private Vector3 newVelocity;

    private bool controlled = true;


    void Start()
    {

        rigidbody = GetComponent<Rigidbody>();
        curJumpDurability = jumpDurability;
        renderer = GetComponent<Renderer>();

        Debug.Log(Input.touchSupported);
    }


    void Update()
    {

        if (controlled)
        {
            newVelocity = rigidbody.velocity;
            newVelocity.x = speed;
            Jump();
            ChangeRenderer();
            Control();
            rigidbody.velocity = newVelocity;
        }

    }


    private bool jumping = false;
    void Control()
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
                }
            }
            else
            {
                if (touch.phase == TouchPhase.Began)
                {
                    jumping = true;
                }
                if ((touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) && currentJumps < maxJumps && curJumpDurability > 0)
                {
                    newVelocity.y = jumpForce;
                    curJumpDurability -= Time.deltaTime;
                }
                else
                {
                    jumping = false;
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    jumping = false;
                    currentJumps++;
                    curJumpDurability = jumpDurability;
                }

            }
            if (!jumping)
            {
                rigidbody.AddForce(Vector3.down * jumpForce);
            }
        }

    }

    void Jump()
    {

        if (Input.GetKey(KeyCode.Space) && currentJumps < maxJumps && curJumpDurability > 0)
        {
            newVelocity.y = jumpForce;
            curJumpDurability -= Time.deltaTime;
        }
        else
        {
            rigidbody.AddForce(Vector3.down * jumpForce);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            currentJumps++;
            curJumpDurability = jumpDurability;
        }

    }


    void ChangeRenderer()
    {
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
        isWinner = true;
        speed = 0;
        levelController.Win("MainMenu");
    }
}
