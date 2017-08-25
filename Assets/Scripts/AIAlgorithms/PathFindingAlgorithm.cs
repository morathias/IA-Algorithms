using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingAlgorithm 
{
    protected List<Node> _closedNodes;
    protected Stack<Vector3> _path;

    Node[,] _nodes;

    protected Node _goal;

    public void start(Node start, Node goal, Node[,] nodes) 
    {
        _nodes = nodes;

        for (int i = 0; i < _nodes.GetLength(0); i++)
        {
            for (int j = 0; j < _nodes.GetLength(1); j++)
            {
                _nodes[i, j].resetParent();
            }
        }

        _closedNodes = new List<Node>();
        
        _goal = goal;

        OnStart();
        _closedNodes.Clear();

        Node currentNode;

        openNode(start);

        while (!isOpenedNodesEmpty())
        {
            currentNode = makeCurrentNode();

            if (currentNode == goal)
            {
                Debug.Log("Closed nodes: " + _closedNodes.Count);
                return;
            }

            else
            {
                closeNode(currentNode);

                findCurrentNodeAdjacents(currentNode);

                foreach (Node neighbour in currentNode.getAdjacentNodes())
                    openNode(neighbour);
            }
        }
    }

    protected virtual void OnStart(){}

    protected virtual void openNode(Node node) { }

    protected virtual void closeNode(Node node){
        _closedNodes.Add(node);
    }

    protected virtual Node makeCurrentNode() { return null; }

    void findCurrentNodeAdjacents(Node currentNode)
    {
        Node left = null;

        if (currentNode.getCol() != 0)
            left = _nodes[currentNode.getCol() - 1, currentNode.getRow()];

        if (left != null) 
        {
            if (!isInOpenedNodes(left) && !_closedNodes.Contains(left) && !left.isWall())
                currentNode.addAdjacentNode(left);
        }
        //--
        Node right = null;

        if (currentNode.getCol() != _nodes.GetLength(0) - 1)
            right = _nodes[currentNode.getCol() + 1, currentNode.getRow()];

        if (right != null)
        {
            if (!isInOpenedNodes(right) && !_closedNodes.Contains(right) && !right.isWall())
                currentNode.addAdjacentNode(right);
        }
        //--
        Node up = null;

        if (currentNode.getRow() != 0)
            up = _nodes[currentNode.getCol(), currentNode.getRow() - 1];

        if (up != null)
        {
            if (!isInOpenedNodes(up) && !_closedNodes.Contains(up) && !up.isWall())
                currentNode.addAdjacentNode(up);
        }
        //--
        Node down = null;

        if (currentNode.getRow() != _nodes.GetLength(1) - 1)
            down = _nodes[currentNode.getCol(), currentNode.getRow() + 1];

        if (down != null)
        {
            if (!isInOpenedNodes(down) && !_closedNodes.Contains(down) && !down.isWall())
                currentNode.addAdjacentNode(down);
        }
    }

    public void buildPath(Node goal) {
        if(_path == null)
            _path = new Stack<Vector3>();
        else
            _path.Clear();

        goal.addToPath(ref _path);
    }

    public Stack<Vector3> getPath()
    {
        return _path;
    }

    protected virtual bool isInOpenedNodes(Node node) { return true; }
    protected virtual bool isOpenedNodesEmpty() { return false; }
}
