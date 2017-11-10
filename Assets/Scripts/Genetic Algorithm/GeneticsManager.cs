using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticsManager : MonoBehaviour {
    public GameObject agent;

    GeneticAlgorithm _algorithm = new GeneticAlgorithm();

    public int agentCount;
    int _generation = 0;
	
	void Start () {
        float pos = -8f;
        for (int i = 0; i < agentCount; i++)
        {
            Instantiate(agent, new Vector3(pos, 9f, 0), Quaternion.identity);
            pos += 1.5f;
        }
	}
    
	void Update () {
		
	}
}
