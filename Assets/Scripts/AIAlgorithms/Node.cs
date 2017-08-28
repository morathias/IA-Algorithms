using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    Node _parent;

    protected List<Node> _adjacentNodes;

    bool _isWall = false;

    protected int _col, _row;

    int _score = 1;
    int _heuristic = 0;

    public Node(int col, int row) {
        _col = col;
        _row = row;
        _adjacentNodes = new List<Node>();
    }

    public virtual void addAdjacentNode(Node neighbour) {
        _adjacentNodes.Add(neighbour);
        neighbour.setParent(this);
        neighbour.setScore(_score + neighbour.getScore());
    }

    public List<Node> getAdjacentNodes() {
        return _adjacentNodes;
    }

    public void setParent(Node parent) {
        _parent = parent;
    }
    public Node getParent() {
        return _parent;
    }
    public void resetParent() {
        _parent = null;
        _adjacentNodes.Clear();
    }

    public void setScore(int score)
    {
        _score = score;
    }
    public int getScore()
    {
        return _score;
    }

    public void setHeuristic(int heuristic)
    {
        _heuristic = heuristic;
    }
    public int getHeuristic()
    {
        return _heuristic;
    }

    public int getCol() { return _col; }
    public int getRow() { return _row; }

    public void makeWall(bool isWall) { _isWall = isWall; }
    public bool isWall() { return _isWall; }

    public void addToPath(ref Stack<Vector3> path) 
    {
        if(_parent != null)
        {
            path.Push(new Vector3(_parent.getCol(), 0 , _parent.getRow()));
            _parent.addToPath(ref path);
        }
    }
}
