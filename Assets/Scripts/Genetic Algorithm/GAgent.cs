using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAgent {

    Chromosome _chromosome;

    int _currentGen = 0;

    public int id;

    public void setChromosome(Chromosome chromosome) {
        _chromosome = chromosome;
    }
    public Chromosome getChromosome(){
        return _chromosome;
    }

    public int getGenIndex() {
        return _currentGen;
    }

    public void nextGen() {
        if(_currentGen < _chromosome.getGenes().Count)
            _currentGen++;
    }
    public void resetGenIndex() {
        _currentGen = 0;
    }
}
