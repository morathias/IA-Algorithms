using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NNLayer {
    public int neuronCount;

    public List<Neuron> neurons = new List<Neuron>();

    public NNLayer(int neuronCount, int inputCountPerNeuron) {
        this.neuronCount = neuronCount;
        System.Random random = new System.Random();
        for (int i = 0; i < neuronCount; i++)
            neurons.Add(new Neuron(inputCountPerNeuron, random));
    }
}
