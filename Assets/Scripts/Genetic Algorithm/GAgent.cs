using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAgent : MonoBehaviour {

    Chromosome _chromosome = new Chromosome();

    int _currentGen = 0;

    public Chromosome getChromosome() {
        return _chromosome;
    }

    public int getGenIndex() {
        return _currentGen;
    }

    public void nextGen() {
        if(_currentGen < _chromosome.getGenes().Count - 1)
            _currentGen++;
    }
}
