using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Algorithm {
    BreadthFirst,
    DepthFirst,
    Dijkstra,
    AStar
}

public class Grid : MonoBehaviour {
    public GameObject tile;
    public InputField widthInFd, heightInFd;
    public Dropdown algorithmDrpDn;
    public Text instructionsTxt;

    int _width, _height;

    GameObject[,] _tiles;
    Node[,] _nodes;

    PathFindingAlgorithm _pathFinderAlgorithm;

    Algorithm _currentAlgorithm;

	void Start () {
        _width = 30;
        _height = 30;
        widthInFd.text = "30";
        heightInFd.text = "30";

        _tiles = new GameObject[_width, _height];
        _nodes = new Node[_width, _height];

        _pathFinderAlgorithm = new BreadthFirst();
        _currentAlgorithm = Algorithm.BreadthFirst;

        buildGrid();
	}

    void buildGrid() {
        for (int x = 0; x < _width; x++){
            for (int y = 0;  y < _height;  y++){
                _nodes[x, y] = new Node(x, y);
                _tiles[x, y] = Instantiate(tile, new Vector3(x, 0, y), Quaternion.identity, gameObject.transform);
            }
        }
    }

    public void resizeGrid() {
        for (int x = 0; x < _width; x++){
            for (int y = 0; y < _height; y++){
                Destroy(_tiles[x, y]);
            }
        }

        _width = int.Parse(widthInFd.text);
        _height = int.Parse(heightInFd.text);

        _tiles = new GameObject[_width, _height];
        _nodes = new Node[_width, _height];

        buildGrid();
    }

    public void selectAlgorithm() {
        _currentAlgorithm = (Algorithm)algorithmDrpDn.value;

        switch (_currentAlgorithm)
        {
            case Algorithm.BreadthFirst:
                _pathFinderAlgorithm = new BreadthFirst();
                instructionsTxt.text = "LMB: creates wall if not created, otherwise deletes it\nWASD: camera navigation";
                break;
            case Algorithm.DepthFirst:
                _pathFinderAlgorithm = new DepthFirst();
                instructionsTxt.text = "LMB: creates wall if not created, otherwise deletes it\nWASD: camera navigation";
                break;
            case Algorithm.Dijkstra:
                _pathFinderAlgorithm = new Dijkstra();
                instructionsTxt.text = "LMB: creates wall if not created, otherwise deletes it\nWASD: camera navigation\nRMB: add Water(3),Mud(6), deletes tile";
                break;
            case Algorithm.AStar:
                _pathFinderAlgorithm = new AStar();
                instructionsTxt.text = "LMB: creates wall if not created, otherwise deletes it\nWASD: camera navigation\nRMB: add Water(3),Mud(6), deletes tile";
                break;
            default:
                break;
        }
    }

    public Stack<Vector3> startAlgorithm(Vector3 start, Vector3 goal) {
        _pathFinderAlgorithm.start(_nodes[(int)start.x, (int)start.z],
                                   _nodes[(int)goal.x, (int)goal.z],
                                   _nodes);

        _pathFinderAlgorithm.buildPath(_nodes[(int)goal.x, (int)goal.z]);

        return _pathFinderAlgorithm.getPath();
    }

    public int getWidth() {
        return _width;
    }
    public int getHeight() {
        return _height;
    }

    public void makeWall(int col, int row, bool isWall) {
        _nodes[col, row].makeWall(isWall);
    }
    public bool isWall(int col, int row) {
        return _nodes[col, row].isWall();
    }

    public int getNodeScore(int col, int row) {
        return _nodes[col, row].getScore();
    }
    public void setNodeScore(int col, int row, int score) {
        _nodes[col, row].setScore(score);
    }
}
