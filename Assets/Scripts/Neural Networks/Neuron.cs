using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron {
    public int inputCount = 0;

    public List<float> weights;

    public Neuron(int inputCount, System.Random random) {
        this.inputCount = inputCount + 1;
        weights = new List<float>();

        for (int i = 0; i < this.inputCount; i++)
            weights.Add(Random.Range(-1f, 1f));
    }
}
