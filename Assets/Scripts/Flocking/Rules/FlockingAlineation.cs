using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingAlineation : FlockingRule {
    public override Vector3 calculateValue(List<Transform> currentBoidNeighbours)
    {
        Vector3 value = new Vector3();
        for (int i = 0; i < currentBoidNeighbours.Count; i++)
            value += currentBoidNeighbours[i].position;

        value /= currentBoidNeighbours.Count;
        Vector3.Normalize(value);

        return value;
    }
}
