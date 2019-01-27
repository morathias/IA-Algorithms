using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NNAgent {

    NNChromosome _chromosome;

    int _currentGen = 0;

    public int id;

    public void setChromosome(NNChromosome chromosome)
    {
        _chromosome = chromosome;
    }
    public NNChromosome getChromosome()
    {
        return _chromosome;
    }

    public int getGenIndex()
    {
        return _currentGen;
    }

    public void nextGen()
    {
        if (_currentGen < _chromosome.getGenes().Count)
            _currentGen++;
    }
    public void resetGenIndex()
    {
        _currentGen = 0;
    }
}
