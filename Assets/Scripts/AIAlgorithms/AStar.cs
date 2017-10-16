using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : PathFindingAlgorithm {
    List<Node> _openedNodes;

    public AStar()
    {
        Debug.Log("AStar");
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

        currentNode.setHeuristic(calculateCurrentHeuristic(currentNode));

        for (int i = 1; i < _openedNodes.Count; i++)
        {
            Node nodeToCheck = _openedNodes[i];
            nodeToCheck.setHeuristic(calculateCurrentHeuristic(nodeToCheck));

            if (nodeToCheck.getScore() + nodeToCheck.getHeuristic() < currentNode.getScore() + currentNode.getHeuristic())
                currentNode = nodeToCheck;
        }

        _openedNodes.Remove(currentNode);
        return currentNode;
    }

    int calculateCurrentHeuristic(Node currentNode) {
        int currentHeuristic = 0;

        if (currentNode.getCol() > _goal.getCol()) {
            for (int i = currentNode.getCol(); i > _goal.getCol(); i--)
                currentHeuristic++;
        }

        else{
            for (int i = currentNode.getCol(); i < _goal.getCol(); i++)
                currentHeuristic++;
        }

        if (currentNode.getRow() > _goal.getRow()) {
            for (int i = currentNode.getRow(); i > _goal.getRow(); i--)
                currentHeuristic++;
        }

        else{
            for (int i = currentNode.getRow(); i < _goal.getRow(); i++)
                currentHeuristic++;
        }

        return currentHeuristic;
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
