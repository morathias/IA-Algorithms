using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GeneticAlgorithm {
    List<Chromosome> _currentGeneration = new List<Chromosome>();
    List<Chromosome> _nextGenerationAgents = new List<Chromosome>();

    const float ELITE_PERCENTAGE = 0.2f;
    float totalScore = 0;
    float _agentCount = 0;

    public GeneticAlgorithm(int agentCount) {
        _agentCount = agentCount;

        System.Random randomValues = new System.Random();

        for (int i = 0; i < _agentCount; i++)
        {
            Chromosome chromosome = new Chromosome(randomValues);
            _currentGeneration.Add(chromosome);
        }
    }

    public List<Chromosome> getCurrentGenerationChromosomes() {
        return _currentGeneration;
    }

    public void evolutionate() {
        totalScore = 0;

        _currentGeneration = _currentGeneration.OrderByDescending((gAgent) => gAgent.getScore()).ToList();
        
        showScores();

        for (int i = 0; i < _currentGeneration.Count; i++)
            totalScore += _currentGeneration[i].getScore();
        
        selectElites();

        Debug.Log("newAgents count: " + _nextGenerationAgents.Count);

        while (_nextGenerationAgents.Count != _agentCount)
        {
            Debug.Log("dentro del wihile");
            startPairing();
        }

        Debug.Log("newAgents count: " + _nextGenerationAgents.Count);

        _currentGeneration.Clear();
        _currentGeneration = null;
        _currentGeneration = _nextGenerationAgents.ToList();
        
        _nextGenerationAgents.Clear();
    }

    public void selectElites()
    {
        Debug.Log("selecting elites");
        float eliteIndex = _currentGeneration.Count * ELITE_PERCENTAGE;

        for (int i = 0; i < (int)eliteIndex; i++)
           _nextGenerationAgents.Add(_currentGeneration[i]);
    }

    public void showScores() {
        for (int i = 0; i < _currentGeneration.Count; i++)
            Debug.Log(_currentGeneration[i].getScore());
    }

    public void startPairing()
    {
        Debug.Log("start pairing");

        Chromosome mom = roulette();
        Chromosome dad = roulette();
        /*while (mom == dad)
        {
            Debug.Log("cant fuck with himself");
            mom = roulette();
            dad = roulette();
        }*/

        Chromosome baby1, baby2;
        pair(ref dad, ref mom, out baby1, out baby2);
        mutate(ref baby1, ref baby2);

        _nextGenerationAgents.Add(baby1);
        _nextGenerationAgents.Add(baby2);
    }

    void pair(ref Chromosome dad, ref Chromosome mom, out Chromosome baby1, out Chromosome baby2)
    {
        int pivote = Random.Range(0, 10);
        List<Gen> baby1Genes = new List<Gen>();
        List<Gen> baby2Genes = new List<Gen>();
        for (int i = 0; i < pivote; i++)
        {
            baby1Genes.Add(dad.getGenes()[i]);
            baby2Genes.Add(mom.getGenes()[i]);
        }

        for (int i = pivote; i < 10; i++)
        {
            baby1Genes.Add(mom.getGenes()[i]);
            baby2Genes.Add(dad.getGenes()[i]);
        }

        baby1 = new Chromosome(baby1Genes);
        baby2 = new Chromosome(baby2Genes);
    }

    void mutate(ref Chromosome baby1, ref Chromosome baby2) {
        for (int i = 0; i < 10; i++)
        {
            int mutateTimePercentage = Random.Range(0, 10);
            
            //Debug.Log("time : " + mutateTimePercentage + " action: " + mutateActionPercentage);

            if (mutateTimePercentage == 0)
            {
                baby1.getGenes()[i].mutateTime(Random.Range(-0.01f, 0.01f));
                //Debug.Log("baby 1 mutated time");
            }
            /*int mutateActionPercentage = Random.Range(0, 10);
            if (mutateActionPercentage == 0)
            {
                baby1.getGenes()[i].mutateAction((LanderAction)Random.Range(0, (int)LanderAction.actionsCount));
                //Debug.Log("baby 1 mutated action");
            }*/
        }

        for (int i = 0; i < 10; i++)
        {
            int mutateTimePercentage = Random.Range(0, 100);
            
            //Debug.Log("time : " + mutateTimePercentage + " action: " + mutateActionPercentage);

            if (mutateTimePercentage == 0)
            {
                baby2.getGenes()[i].mutateTime(Random.Range(-0.01f, 0.01f));
                //Debug.Log("baby 2 mutated time");
            }

            /*int mutateActionPercentage = Random.Range(0, 100);
            if (mutateActionPercentage == 0)
            {
                baby2.getGenes()[i].mutateAction((LanderAction)Random.Range(0, (int)LanderAction.actionsCount));
                //Debug.Log("baby 2 mutated action");
            }*/
        }
    }

    Chromosome roulette() {
        int selectFitness = Random.Range(0, (int)totalScore);
        int selectedAgent = 0;
        int selectorRange = 0;

        for (int i = 0; i < _currentGeneration.Count; i++)
        {
            selectorRange += (int)_currentGeneration[i].getScore();

            if (selectorRange > selectFitness) {
                selectedAgent = i;
                break;
            }
        }

        Debug.Log(selectedAgent + " selected");
        return _currentGeneration[selectedAgent];
    }
}
