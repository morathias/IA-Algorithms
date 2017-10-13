using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {
    public Transform _target;

    public float rotationSpeed;
    public float movingSpeed;

    List<Transform> _neighbours;
    List<FlockingRule> _rules;

    private void Awake()
    {
        _neighbours = new List<Transform>();
        _rules = new List<FlockingRule>();
    }

    void Update () {
        Quaternion rotation = Quaternion.LookRotation(_target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        if (Vector3.Distance(_target.position, transform.position) <= 0.1f)
            return;

        transform.Translate(transform.forward * movingSpeed * Time.deltaTime, Space.World);
	}

    Vector3 calculateAlignment() {
        return Vector3.zero;
    }

    Vector3 calculateCohesion() {
        return Vector3.zero;
    }

    Vector3 calculateSeparation() {
        return Vector3.zero;
    }

    public void addNeighbour(Transform neighbour) {
        _neighbours.Add(neighbour);
    }

    public void clearNeighbours() {
        _neighbours.Clear();
    }
}
