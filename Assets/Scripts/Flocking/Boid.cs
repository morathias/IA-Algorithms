using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {
    public Transform _target;

    Vector3 direction;

    public float rotationSpeed;
    public float movingSpeed;

    List<Transform> _neighbours;
    List<FlockingRule> _rules;
    [Range(1,5)]
    public float cohesionWeight, separationWeight, AlignmentWeight;

    private void Awake()
    {
        _neighbours = new List<Transform>();
        _rules = new List<FlockingRule>();
    }

    void Start() {
        FlockingCohesion cohesion = new FlockingCohesion();
        FlockingSeparation separation = new FlockingSeparation();
        FlockingAlineation alineation = new FlockingAlineation();

        _rules.Add(cohesion);
        _rules.Add(separation);
        _rules.Add(alineation);
    }

    void Update () {
        direction = (calculateCohesion() + calculateSeparation() + calculateAlignment()) / _rules.Count;

        Quaternion rotation = Quaternion.LookRotation(_target.position - (transform.position - direction));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        //Debug.DrawLine(transform.position, transform.position - direction.normalized * -1);

        transform.Translate((direction.normalized + transform.forward).normalized * movingSpeed * Time.deltaTime, Space.World);

        //transform.rotation = Quaternion.LookRotation(direction);
	}

    Vector3 calculateCohesion()
    {
        Vector3 finalValue = Vector3.zero;
        Vector3 value = _rules[0].calculateValue(gameObject.transform, _neighbours);
        finalValue = value.normalized * cohesionWeight;
        Debug.DrawLine(transform.position, transform.position - finalValue * -1, Color.blue);
        return finalValue;
    }

    Vector3 calculateSeparation()
    {
        Vector3 finalValue = Vector3.zero;
        Vector3 value = _rules[1].calculateValue(gameObject.transform, _neighbours);
        finalValue = value.normalized * separationWeight;
        Debug.DrawLine(transform.position, transform.position - finalValue * -1, Color.red);
        return finalValue;
    }

    Vector3 calculateAlignment() {
        Vector3 finalValue = Vector3.zero;
        Vector3 value = _rules[2].calculateValue(gameObject.transform, _neighbours);
        finalValue = value.normalized * AlignmentWeight;
        Debug.DrawLine(transform.position, transform.position - finalValue * -1, Color.yellow);
        return finalValue;
    }

    public void addNeighbour(Transform neighbour) {
        _neighbours.Add(neighbour);
    }

    public void clearNeighbours() {
        _neighbours.Clear();
    }
}
