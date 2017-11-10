using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chromosome {
    List<Gen> _genes = new List<Gen>();

    public Chromosome()
    {
        Debug.Log((int)LanderAction.actionsCount);
        System.Random random = new System.Random();

        for (int i = 0; i < (int)LanderAction.actionsCount; i++)
        {
            float time = random.Next(0, 10);
            LanderAction action = (LanderAction)random.Next(0, (int)LanderAction.actionsCount);

            Debug.Log(action +" " + time);

            Gen gen = new Gen(action, time);
            _genes.Add(gen);
        }
    }

    public List<Gen> getGenes() {
        return _genes;
    }
}
