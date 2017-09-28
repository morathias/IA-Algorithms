using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    public int speed;

	void Update () {
        gameObject.transform.Translate(new Vector3(Input.GetAxis("Horizontal"), -Input.GetAxis("Mouse ScrollWheel") * 10, Input.GetAxis("Vertical")) * 
                                       speed * Time.deltaTime, 
                                       Space.World);
	}
}
