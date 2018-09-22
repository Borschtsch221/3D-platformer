using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    public float rotateSpeed = 50;

	void Update () {
        transform.RotateAroundLocal(Vector3.up, rotateSpeed);
	}
}
