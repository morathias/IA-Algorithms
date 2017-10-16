using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingCohesion : FlockingRule {
    public override Vector3 calculateValue(Transform currentBoid, List<Transform> currentBoidNeighbours)
    {
        Vector3 value = Vector3.zero;

        for (int i = 0; i < currentBoidNeighbours.Count; i++)
            value += currentBoidNeighbours[i].position;

        value /= currentBoidNeighbours.Count;
        value -= currentBoid.position;
        return value;
    }
}
