using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Chromosome {
    List<Gen> _genes;

    float _score = 0;

    public Chromosome(System.Random random)
    {
        _genes = new List<Gen>();

        for (int i = 0; i < (int)LanderAction.actionsCount + 6; i++)
        {
            float time = random.Next(0, 3);
            LanderAction action = (LanderAction)random.Next(0, (int)LanderAction.actionsCount);

            Gen gen = new Gen(action, time);
            _genes.Add(gen);
        }
    }

    public Chromosome(List<Gen> newGenes) {
        _genes = newGenes.ToList();
    }

    public void inSimulation(Vector3 velocity, Vector3 distance, float flyingTime, int reachedMultiplier, float angle) {
        //Debug.Log("distance " + distance.magnitude + " velocity : " + velocity.magnitude);
        /*if (velocity.magnitude > 0)
            _score = (22f - distance.magnitude) / velocity.magnitude;

        else
            _score = 0;*/
        _score = (22f - distance.magnitude) * reachedMultiplier;
    }

    public List<Gen> getGenes() {
        return _genes;
    }
    public void setGenes(List<Gen> genes) {
        _genes.Clear();
        _genes = null;
        _genes = genes.ToList();
    }

    public float getScore() {
        return _score;
    }
}
