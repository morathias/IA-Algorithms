using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reseter : MonoBehaviour {
    Vector3 _startingPos;
	
	void Start () {
        _startingPos = gameObject.transform.position;
	}

    public void reset() {
        gameObject.transform.position = _startingPos;
    }
}
