using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthFirst : PathFindingAlgorithm {
    Stack<Node> _openedNodes;
    public DepthFirst()
    {
        _openedNodes = new Stack<Node>();
        Debug.Log("DepthFirst");
    }

    protected override Node makeCurrentNode()
    {
        Node currentNode = _openedNodes.Pop();

        return currentNode;
    }

    protected override void openNode(Node node)
    {
        _openedNodes.Push(node);
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
