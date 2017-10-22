using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlockingRule {
    public abstract Vector3 calculateValue(Transform currentBoid, List<Transform> currentBoidNeighbours);
}
