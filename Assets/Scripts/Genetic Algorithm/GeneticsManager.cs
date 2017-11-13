using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneticsManager : MonoBehaviour {
    public GameObject agent;
    List<GAgent> _landers = new List<GAgent>();
    List<Lander> _objects = new List<Lander>();

    GeneticAlgorithm _algorithm;

    public Text generationTxt;
    public Text simulationTimeTxt;

    public int agentCount = 8;
    public float simulationTime = 10f;
    public float timeScale = 1f;
    float _currentSimulationTime;
    int _generation = 1;
	
	void Start () {
        _algorithm = new GeneticAlgorithm(agentCount);
        generatePopulation();
        _currentSimulationTime = simulationTime;
        generationTxt.text = "Generation: " + _generation;
        simulationTimeTxt.text = "Simulation Time: " + _currentSimulationTime;
	}

    void FixedUpdate() {
        Time.timeScale = timeScale;
        _currentSimulationTime -= Time.fixedDeltaTime;
        simulationTimeTxt.text = "Simulation Time: " + _currentSimulationTime;

        if (_currentSimulationTime <= 0)
        {
            _algorithm.evolutionate();
            generatePopulation(_algorithm.getCurrentGenerationChromosomes());

            _currentSimulationTime = simulationTime;
            _generation++;
            generationTxt.text = "Generation: " + _generation;
        }
    }

    void generatePopulation() {
        List<Chromosome> currentGeneration = _algorithm.getCurrentGenerationChromosomes();

        for (int i = 0; i < agentCount; i++)
        {
            Vector3 startPos = new Vector3(-13f, 15f, 0);
            GameObject lander = Instantiate(agent, startPos, Quaternion.identity);

            Lander landerComponent = lander.GetComponent<Lander>();
            landerComponent.setStartPosition(startPos);

            GAgent gLander = new GAgent();
            gLander.setChromosome(currentGeneration[i]);
            _landers.Add(gLander);
            landerComponent.setGAgent(gLander);

            _objects.Add(landerComponent);
        }
    }

    void generatePopulation(List<Chromosome> newChromosomes) {
        for (int i = 0; i < newChromosomes.Count; i++)
        {
            _objects[i].reset();
            _landers[i].setChromosome(newChromosomes[i]);
            _objects[i].setGAgent(_landers[i]);
        }
    }
    
	void Update () {
		
	}
}
