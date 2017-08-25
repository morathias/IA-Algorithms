using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour {
    Stack<Vector3> _path;
    Vector3 _currentPositionToMove;
    Vector3 _startPosition;

    public float speed;
    bool _startMoving = false;

    void Start() {
        _startPosition = transform.position;
    }

	void Update () {
        if (_startMoving)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, _currentPositionToMove, speed * Time.deltaTime);
            transform.position = pos;

            if(pos == _currentPositionToMove)
            {
                if (_path.Count > 0)
                    _currentPositionToMove = _path.Pop();
            }
        }
	}

    public void setPath(Stack<Vector3> path) {
        _path = path;
        _currentPositionToMove = _path.Pop();
        _startMoving = true;
    }
    public void reset() {
        _currentPositionToMove = _startPosition;
        _startMoving = false;
        _path.Clear();
    }
}
