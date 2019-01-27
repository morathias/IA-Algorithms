using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NNGeneticAlgorithm {

    List<NNChromosome> _currentGeneration = new List<NNChromosome>();
    List<NNChromosome> _nextGenerationAgents = new List<NNChromosome>();

    const float ELITE_PERCENTAGE = 0.2f;
    float totalScore = 0;
    float _agentCount = 0;

    public NNGeneticAlgorithm(int agentCount)
    {
        _agentCount = agentCount;
    }

    public List<NNChromosome> getCurrentGenerationChromosomes()
    {
        return _currentGeneration;
    }

    public void evolutionate(List<NNChromosome> currentGeneration)
    {
        _currentGeneration = currentGeneration.ToList();
        totalScore = 0;

        _currentGeneration = _currentGeneration.OrderByDescending((gAgent) => gAgent.getScore()).ToList();

        showScores();

        for (int i = 0; i < _currentGeneration.Count; i++)
            totalScore += _currentGeneration[i].getScore();

        selectElites();

        while (_nextGenerationAgents.Count != _agentCount)
        {
            startPairing();
        }

        _currentGeneration.Clear();
        _currentGeneration = null;
        _currentGeneration = _nextGenerationAgents.ToList();

        for (int i = 0; i < _currentGeneration.Count; i++)
        {
            _currentGeneration[i].agentName = "agent" + i;
        }

        _nextGenerationAgents.Clear();
    }

    public void selectElites()
    {
        Debug.Log("selecting elites");
        float eliteIndex = _currentGeneration.Count * ELITE_PERCENTAGE;

        for (int i = 0; i < (int)eliteIndex; i++)
            _nextGenerationAgents.Add(_currentGeneration[i]);
    }

    public void showScores()
    {
        for (int i = 0; i < _currentGeneration.Count; i++)
            Debug.Log(_currentGeneration[i].agentName + " score: " + _currentGeneration[i].getScore());
    }

    public void startPairing()
    {
        Debug.Log("start pairing");
        
        NNChromosome mom = roulette();
        NNChromosome dad = roulette();
        /*while (mom == dad)
        {
            Debug.Log("cant fuck with himself");
            mom = roulette();
            dad = roulette();
        }*/

        NNChromosome baby1, baby2;
        pair(ref dad, ref mom, out baby1, out baby2);
        mutate(ref baby1, ref baby2);

        _nextGenerationAgents.Add(baby1);
        _nextGenerationAgents.Add(baby2);
    }

    void pair(ref NNChromosome dad, ref NNChromosome mom, out NNChromosome baby1, out NNChromosome baby2)
    {
        int pivote = Random.Range(0, dad.getGenes().Count + 1);
        List<NNGen> baby1Genes = new List<NNGen>();
        List<NNGen> baby2Genes = new List<NNGen>();
        for (int i = 0; i < pivote; i++)
        {
            baby1Genes.Add(dad.getGenes()[i]);
            baby2Genes.Add(mom.getGenes()[i]);
        }

        for (int i = pivote; i < dad.getGenes().Count; i++)
        {
            baby1Genes.Add(mom.getGenes()[i]);
            baby2Genes.Add(dad.getGenes()[i]);
        }

        baby1 = new NNChromosome(baby1Genes);
        baby2 = new NNChromosome(baby2Genes);
    }

    void mutate(ref NNChromosome baby1, ref NNChromosome baby2)
    {
        for (int i = 0; i < baby1.getGenes().Count; i++)
        {
            int mutatePercentage = Random.Range(0, 100);

            if (mutatePercentage < 1) {
                Debug.Log("baby1 mutated");
                baby1.getGenes()[i].weight += Random.Range(-0.1f, 0.1f);
            }
        }

        for (int i = 0; i < baby2.getGenes().Count; i++)
        {
            int mutatePercentage = Random.Range(0, 100);

            if (mutatePercentage < 1)
            {
                Debug.Log("baby2 mutated");
                baby2.getGenes()[i].weight += Random.Range(-0.1f, 0.1f);
            }
        }
    }

    NNChromosome roulette()
    {
        int selectFitness = Random.Range(0, (int)totalScore);
        int selectedAgent = 0;
        int selectorRange = 0;
        for (int i = 0; i < _currentGeneration.Count; i++)
        {
            selectorRange += (int)_currentGeneration[i].getScore();

            if (selectorRange > selectFitness)
            {
                selectedAgent = i;
                break;
            }
        }

        Debug.Log(selectedAgent + " selected");
        return _currentGeneration[selectedAgent];
    }
}
