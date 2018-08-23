using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCharacter : MonoBehaviour {


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
    private float currentTime;


    public CameraShake cameraShake;
    public CameraFollow cameraFollow;

    public Animator animation;


	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        currentTime = jumpDurability;
        renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        float x = Input.GetAxis("Horizontal");
        Jump();
        ChangeRenderer();

        //rigidbody.MovePosition(transform.position + new Vector3( speed * Time.deltaTime, 0, 0));
        Vector3 newVelocity = rigidbody.velocity;
        newVelocity.x =x* speed;
        rigidbody.velocity = newVelocity;
        //rigidbody.AddForce(new Vector3(speed, 0, 0));
	}



    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && currentJumps < maxJumps && currentTime > 0)
        {
            //rigidbody.AddForce(new Vector3(0, jumpForce, 0));
            //rigidbody.velocity = new Vector3(0, jumpForce, 0);
            Vector3 newVelocity = rigidbody.velocity;
            newVelocity.y = jumpForce;
            rigidbody.velocity = newVelocity;
            currentTime -= Time.deltaTime;
        }
        else
        {
            rigidbody.AddForce(Vector3.down * jumpForce);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            currentJumps++;
            currentTime = jumpDurability;
            
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
        if (cameraShake)
        {
            StartCoroutine(cameraShake.Shake(.15f, .4f));
            if (animation)
            {
                animation.enabled = true;
            }
        }
        if (cameraFollow)
        {
            cameraFollow.follow = false;
            
        }
        renderer.enabled = false;
        StartCoroutine(WaitAndLoadLevel(2f, Application.loadedLevelName));

    }

    public void Win()
    {
        
        levelController.LoadLevel(Application.loadedLevelName);
        //StartCoroutine(WaitAndLoadLevel(2f, Application.loadedLevelName));
    }

    IEnumerator WaitAndLoadLevel(float time, string levelName)
    {
        yield return new WaitForSeconds(time);
        Application.LoadLevel(levelName);
    }


}
