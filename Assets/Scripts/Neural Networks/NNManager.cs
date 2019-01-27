using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NNManager : MonoBehaviour {

    public int hiddenLayersCount = 0;

    public int outputNeuronsCount = 1;

    public GameObject agent;
    List<NNAgent> _landers = new List<NNAgent>();
    List<NNLander> _objects = new List<NNLander>();

    NNGeneticAlgorithm _algorithm;

    public Text generationTxt;
    public Text simulationTimeTxt;
    public Font font;

    public int agentCount = 8;
    public float simulationTime = 10f;
    public float timeScale = 1f;
    float _currentSimulationTime;
    int _generation = 1;

    public RectTransform scoresContainer;

    public PlatformPos platformToPos;

    void Start()
    {
        generatePopulation();
        _currentSimulationTime = simulationTime;
        generationTxt.text = "Generation: " + _generation;
        simulationTimeTxt.text = "Simulation Time: " + _currentSimulationTime;
    }

    void FixedUpdate()
    {
        Time.timeScale = timeScale;
        _currentSimulationTime -= Time.fixedDeltaTime;
        simulationTimeTxt.text = "Simulation Time: " + _currentSimulationTime;

        if (_currentSimulationTime <= 0)
        {
            List<NNChromosome> lastGeneration = new List<NNChromosome>();
            for (int i = 0; i < agentCount; i++)
            {
                lastGeneration.Add(_landers[i].getChromosome());
            }
            _algorithm.evolutionate(lastGeneration);
            generatePopulation(_algorithm.getCurrentGenerationChromosomes());

            _currentSimulationTime = simulationTime;
            _generation++;
            generationTxt.text = "Generation: " + _generation;

            platformToPos.reposition();
        }
    }
    void generatePopulation()
    {
        List<NNChromosome> currentGeneration = new List<NNChromosome>();

        for (int i = 0; i < agentCount; i++)
        {
            Vector3 startPos = new Vector3(-13f, 15f, 0);
            GameObject lander = Instantiate(agent, startPos, Quaternion.Euler(0f, 0f, -90f));
            lander.name = "agent" + i;

            NNLander landerComponent = lander.GetComponent<NNLander>();

            GameObject scoreTxtObj = new GameObject();
            scoreTxtObj.transform.parent = scoresContainer;
            RectTransform scoreTxtRect = scoreTxtObj.AddComponent<RectTransform>();
            scoreTxtRect.localPosition = new Vector2(0f, i * -20f);
            Text scoreTxt = scoreTxtObj.AddComponent<Text>();
            scoreTxt.font = font;
            scoreTxt.fontSize = 10;


            landerComponent.setScoreTxt(scoreTxt);

            landerComponent.setStartPosition(startPos);
            NeuralNetwork brain = new NeuralNetwork(4, outputNeuronsCount, hiddenLayersCount, 4);
            brain.createNetwork();
            landerComponent.setNeuralNetwork(brain);
            NNChromosome chromosome = new NNChromosome();
            chromosome.agentName = lander.name;
            

            for (int j = 0; j < brain.getTotalWeightsCount(); j++)
            {
                NNGen gen = new NNGen();
                gen.weight = brain.getWeights()[j];
                chromosome.getGenes().Add(gen);
            }

            currentGeneration.Add(chromosome);

            NNAgent gLander = new NNAgent();
            gLander.setChromosome(chromosome);
            _landers.Add(gLander);
            landerComponent.setGAgent(gLander);

            _objects.Add(landerComponent);
        }
        _algorithm = new NNGeneticAlgorithm(agentCount);
    }

    void generatePopulation(List<NNChromosome> newChromosomes)
    {
        for (int i = 0; i < newChromosomes.Count; i++)
        {
            _objects[i].reset();
            _landers[i].setChromosome(newChromosomes[i]);
            _objects[i].name = newChromosomes[i].agentName;
            List<float> newWeights = new List<float>();
            for (int j = 0; j < newChromosomes[i].getGenes().Count; j++)
            {
                newWeights.Add(newChromosomes[i].getGenes()[j].weight);
            }
            _objects[i].getBrain().setWeights(newWeights);
            _objects[i].setGAgent(_landers[i]);
        }
    }
}
