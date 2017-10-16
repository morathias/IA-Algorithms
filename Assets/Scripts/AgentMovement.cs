using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour {
    protected Stack<Vector3> _path;
    protected Vector3 _currentPositionToMove;
    Vector3 _startPosition;

    protected Transform _mesh;

    public float speed;
    protected bool _startMoving = false;

    protected virtual void Start() {
        _startPosition = transform.position;
        _mesh = transform.GetChild(0);
    }

	protected virtual void Update () {
        if (_startMoving)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, _currentPositionToMove, speed * Time.deltaTime);
            transform.position = pos;
            
            _mesh.LookAt(_currentPositionToMove + new Vector3(0.5f, 0, 0.5f));

            if(pos == _currentPositionToMove)
            {
                if (_path.Count > 0)
                    _currentPositionToMove = _path.Pop();

                else
                    _startMoving = false;
            }
            
        }
	}

    public void reset() {
        _currentPositionToMove = _startPosition;
        _startMoving = false;
        _path.Clear();
    }
}
