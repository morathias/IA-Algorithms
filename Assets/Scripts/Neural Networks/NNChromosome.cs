using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NNChromosome {

	List<NNGen> _genes;

    float _score = 0;
    float _angleScore = 0;

    public string agentName = "";

    public NNChromosome()
    {
        _genes = new List<NNGen>();
    }

    public NNChromosome(List<NNGen> newGenes) {
        _genes = newGenes.ToList();
    }

    public void inSimulation(Vector3 velocity, Vector3 distance, float flyingTime, int reachedMultiplier, Vector3 upVec) {
        _score = ((22f - distance.magnitude) + _angleScore) * reachedMultiplier;
        //_score = ((22f - distance.magnitude)) * reachedMultiplier;
        float angle = Vector3.Angle(distance.normalized, upVec);
        if (angle <= 25f || angle >= 335f)
            _angleScore += 0.1f * velocity.magnitude;
        else
        {
            _angleScore -= 0.01f;
        }
        //Debug.Log(_score);
    }

    public List<NNGen> getGenes() {
        return _genes;
    }
    public void setGenes(List<NNGen> genes) {
        _genes.Clear();
        _genes = null;
        _genes = genes.ToList();
    }

    public float getScore() {
        return _score;
    }

    public void ResetScore() {
        _score = 0;
        _angleScore = 0;
    }
}
