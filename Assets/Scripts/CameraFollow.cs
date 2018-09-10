using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {


    public Transform lookAt;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;
    public bool follow = true;
    public bool smooth = true;

	void Start () {
	}
	

    void FixedUpdate()
    {
        if (follow)
        {
            Vector3 desiredPosition = lookAt.position + offset;
            if (smooth)
            {
                transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            }
            else
            {
                transform.position = desiredPosition;
            }
            transform.LookAt(lookAt);
        }
        if(Input.GetKeyDown(KeyCode.A)){
            StartCoroutine(Zoom());
        }

    }

    [Header("Zoom")]
    public float turnTime = 0.2f;
    public float rotateSpeed = 1000f;
    public float translateSpeed = 5f;
    public IEnumerator Zoom(){
        follow = false;
        while(turnTime>0){
            transform.Rotate(0,0,rotateSpeed/10*Time.deltaTime);
            transform.Translate(0,0,translateSpeed*Time.deltaTime);
            turnTime-=Time.deltaTime;
            yield return null;

        }
        turnTime = 0.7f;
        while(turnTime>0){
            transform.Rotate(0,0,-rotateSpeed*Time.deltaTime);
            transform.Translate(0,0,translateSpeed*Time.deltaTime);
            turnTime-=Time.deltaTime;
            yield return null;

        }
        follow = true;
    }
}
