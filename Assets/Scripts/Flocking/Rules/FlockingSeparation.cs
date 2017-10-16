using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingSeparation : FlockingRule {
    public override Vector3 calculateValue(Transform currentBoid, List<Transform> currentBoidNeighbours)
    {
        Vector3 value = Vector3.zero;

        for (int i = 0; i < currentBoidNeighbours.Count; i++)
            value += currentBoidNeighbours[i].position - currentBoid.position;

        value /= currentBoidNeighbours.Count;
        value *= -1;
        return value;
    }
}
