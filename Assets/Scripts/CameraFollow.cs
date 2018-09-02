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

    public IEnumerator Zoom(){
        float time = 0.3f;
        follow = false;
        while(time>0){
            transform.Rotate(0,0,100*Time.deltaTime);
            transform.Translate(0,0,5f*Time.deltaTime);
            time-=Time.deltaTime;
            yield return null;

        }
        time = 0.7f;
        while(time>0){
            transform.Rotate(0,0,-1000*Time.deltaTime);
            transform.Translate(0,0,5f*Time.deltaTime);
            time-=Time.deltaTime;
            yield return null;

        }
        follow = true;
    }
}
