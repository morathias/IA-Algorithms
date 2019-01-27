using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NeuralNetwork {
    int _inputsCount;
    int _outputsCount;

    int _hiddenLayersCount;
    int _neuronsPerHiddenLayer;

    int bias = -1;

    List<NNLayer> _layers = new List<NNLayer>();

    public NeuralNetwork(int inputsCount, int outputsCount, int hiddenLayersCount, int neuronsPerHiddenLayer) {
        _inputsCount = inputsCount;
        _outputsCount = outputsCount;
        _hiddenLayersCount = hiddenLayersCount;
        _neuronsPerHiddenLayer = neuronsPerHiddenLayer;
    }

    public void createNetwork() {
        NNLayer inputLayer = new NNLayer(_inputsCount, _inputsCount);
        _layers.Add(inputLayer);

        NNLayer outputLayer;

        if (_hiddenLayersCount > 0){
            NNLayer firstHiddenLayer = new NNLayer(_neuronsPerHiddenLayer, _inputsCount);
            _layers.Add(firstHiddenLayer);

            for (int i = 1; i < _hiddenLayersCount; i++){
                NNLayer hiddenLayer = new NNLayer(_neuronsPerHiddenLayer, _neuronsPerHiddenLayer);
                _layers.Add(hiddenLayer);
            }

            outputLayer = new NNLayer(_outputsCount, _neuronsPerHiddenLayer);
            _layers.Add(outputLayer);
        }

        else {
            outputLayer = new NNLayer(_outputsCount, _inputsCount);
            _layers.Add(outputLayer);
        }
    }

    public List<float> getWeights() {
        List<float> totalWeights = new List<float>();

        for (int i = 0; i < _layers.Count; i++){
            for (int j = 0; j < _layers[i].neuronCount; j++){
                for (int k = 0; k < _layers[i].neurons[j].weights.Count; k++)
                    totalWeights.Add(_layers[i].neurons[j].weights[k]);
            }
        }

        return totalWeights;
    }

    public int getTotalWeightsCount() {
        int totalWeightsCount = 0;

        for (int i = 0; i < _layers.Count; i++)
        {
            for (int j = 0; j < _layers[i].neuronCount; j++)
            {
                totalWeightsCount += _layers[i].neurons[j].weights.Count;   
            }
        }

        return totalWeightsCount;
    }

    public void setWeights(List<float> newWeights) {
        int totalWeightsIndex = 0;
        
        for (int i = 0; i < _layers.Count; i++){
            for (int j = 0; j < _layers[i].neuronCount; j++){
                for (int k = 0; k < _layers[i].neurons[j].weights.Count; k++)
                {
                    _layers[i].neurons[j].weights[k] = newWeights[totalWeightsIndex++];
                }
            }
        }
    }

    float sigmoid(float activation, float response) {
        return 1 / (1 + Mathf.Exp((-activation / response)));
    }

    public List<float> update(List<float> inputs) {
        List<float> outputs;

        int totalWeightsIndex = 0;

        if (inputs.Count != _inputsCount)
        {
            return null;
        }

        outputs = new List<float>();
        for (int i = 0; i < _layers.Count; ++i)
        {
            if (i > 0)
                inputs = outputs.ToList();

            outputs.Clear();

            totalWeightsIndex = 0;

            for (int j = 0; j < _layers[i].neuronCount; ++j)
            {
                float netInput = 0;

                int inputCount = _layers[i].neurons[j].inputCount;
                for (int k = 0; k < inputCount - 1; ++k)
                    netInput += _layers[i].neurons[j].weights[k] * inputs[totalWeightsIndex++];

                netInput += _layers[i].neurons[j].weights[inputCount - 1] * bias;

                outputs.Add(sigmoid(netInput, 1));

                totalWeightsIndex = 0;
            }
        }
        return outputs;
    }
}
