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

    }
}
