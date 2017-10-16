using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : MonoBehaviour
{
    List<Boid> _boids;

    float _recalculateTimer = 0.10f;

    public float range;

    private void Awake()
    {
        _boids = new List<Boid>();
    }

    void Start()
    {
        //get all boids
        GameObject[] boids = GameObject.FindGameObjectsWithTag("Boid");

        for (int i = 0; i < boids.Length; i++)
            _boids.Add(boids[i].GetComponent<Boid>());

        Debug.Log(_boids.Count);
    }

    void Update()
    {
        clearBoidsNeighbours();
        setBoidNeighbourgs();
    }

    void setBoidNeighbourgs()
    {
        for (int i = 0; i < _boids.Count; i++)
        {
            for (int j = i + 1; j < _boids.Count; j++)
            {
                if (Vector3.Distance(_boids[i].transform.position, _boids[j].transform.position) < range)
                {
                    _boids[i].addNeighbour(_boids[j].transform);
                    _boids[j].addNeighbour(_boids[i].transform);
                }
            }
        }
    }

    void clearBoidsNeighbours() {
        for (int i = 0; i < _boids.Count; i++)
            _boids[i].clearNeighbours();
    }
}
