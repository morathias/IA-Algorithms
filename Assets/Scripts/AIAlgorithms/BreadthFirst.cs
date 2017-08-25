using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirst : PathFindingAlgorithm {
    Queue<Node> _openedNodes;

    public BreadthFirst()
    {
        Debug.Log("BreadthFirst");
        _openedNodes = new Queue<Node>();
    }

    protected override void OnStart()
    {
        _openedNodes.Clear();
    }

    protected override Node makeCurrentNode()
    {
        Node currentNode = _openedNodes.Dequeue();

        return currentNode;
    }

    protected override void openNode(Node node)
    {
        _openedNodes.Enqueue(node);
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
