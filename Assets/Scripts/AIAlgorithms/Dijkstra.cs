using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : PathFindingAlgorithm {
    List<Node> _openedNodes;

    public Dijkstra() 
    {
        Debug.Log("Dijkstra");
        _openedNodes = new List<Node>();
    }

    protected override Node makeCurrentNode()
    {
        Node currentNode = _openedNodes[0];

        if (_openedNodes.Count < 2)
        {
            _openedNodes.Remove(currentNode);
            return currentNode;
        }

        for (int i = 1; i < _openedNodes.Count; i++)
        {
            Node nodeToCheck = _openedNodes[i];

            if (nodeToCheck.getScore() < currentNode.getScore())
                currentNode = nodeToCheck;
        }

        _openedNodes.Remove(currentNode);
        return currentNode;
    }

    protected override void openNode(Node node)
    {
        _openedNodes.Add(node);
    }

    protected override bool isInOpenedNodes(Node node)
    {
        return _openedNodes.Contains(node);
    }

    protected override bool isOpenedNodesEmpty()
    {
        if (_openedNodes.Count <= 0)
            return true;

        return false;
    }
}
