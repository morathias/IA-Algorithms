using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlockingRule : MonoBehaviour {
    public abstract Vector3 calculateValue(Transform currentBoid, List<Transform> currentBoidNeighbours);
}
