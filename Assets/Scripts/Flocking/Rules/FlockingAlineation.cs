using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingAlineation : FlockingRule {
    public override Vector3 calculateValue(Transform currentBoid, List<Transform> currentBoidNeighbours)
    {
        Vector3 value = new Vector3();
        for (int i = 0; i < currentBoidNeighbours.Count; i++)
            value += currentBoidNeighbours[i].forward;

        value /= currentBoidNeighbours.Count;
        return value;
    }
}
