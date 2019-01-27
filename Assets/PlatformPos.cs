using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPos : MonoBehaviour {
    public Transform positions;

    void Start() {
        reposition();
    }

    public void reposition() {
        transform.position = positions.GetChild(Random.Range(0, positions.childCount)).position;
    }
}
