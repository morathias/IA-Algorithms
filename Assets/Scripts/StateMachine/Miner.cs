using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Events
{
    StartMoving,
    Finished,
    ReachedMine,
    ReachedCastle,
    ReachedPosition,
    EventsCount
}

public enum States
{
    Idle,
    Moving,
    Mining,
    Deploying,
    StatesCount
} 

public class Miner : AgentMovement {
    public Grid grid;

    public GameObject goal;
    Node goalNode;

    FSM _stateMachine;

    States _state;

    int _goldAmount = 0;
    public Text goldAmountTxt;
	
	protected override void Start () {
        base.Start();
        _stateMachine = GetComponent<FSM>();
        _stateMachine.init((int)States.StatesCount, (int)Events.EventsCount);

        _state = (States)_stateMachine.getState();

        _stateMachine.setRelation((int)States.Idle, (int)Events.StartMoving, (int)States.Moving);
        _stateMachine.setRelation((int)States.Moving, (int)Events.ReachedPosition, (int)States.Idle);
        _stateMachine.setRelation((int)States.Moving, (int)Events.ReachedCastle, (int)States.Deploying);
        _stateMachine.setRelation((int)States.Deploying, (int)Events.Finished, (int)States.Idle);
        _stateMachine.setRelation((int)States.Moving, (int)Events.ReachedMine, (int)States.Mining);
        _stateMachine.setRelation((int)States.Mining, (int)Events.Finished, (int)States.Idle);
	}

    protected override void Update()
    {
        _state = (States)_stateMachine.getState();

        switch (_state)
        {
            case States.Idle:
                idle();
                break;

            case States.Moving:
                base.Update();
                moving();
                break;

            case States.Mining:
                mining();
                break;

            case States.Deploying:
                deploying();
                break;
        }
    }

    void idle() {
        if (Input.GetMouseButtonDown(1))
        {
            _path = grid.startAlgorithm(transform.position, mouseToWorld());
            _currentPositionToMove = _path.Pop();
            _startMoving = true;
            goal.transform.position = mouseToWorld();

            goalNode = grid.getNode((int)goal.transform.position.x, (int)goal.transform.position.z);

            _stateMachine.setEvent((int)Events.StartMoving);
        }
    }

    void moving() {
        if (transform.position == _currentPositionToMove)
        {
            _path.Clear();
            Debug.Log(goalNode.getScore());

            if (goalNode.getScore() >= 100 && goalNode.getScore() <= 1000)
                _stateMachine.setEvent((int)Events.ReachedMine);
            else if (goalNode.getScore() >= 1000)
                _stateMachine.setEvent((int)Events.ReachedCastle);
            else
                _stateMachine.setEvent((int)Events.ReachedPosition);
        }
    }

    void mining() {
        _goldAmount++;
        goldAmountTxt.text = _goldAmount.ToString();
        if (_goldAmount >= 1000)
            _stateMachine.setEvent((int)Events.Finished);
    }

    void deploying() {
        _goldAmount--;
        goldAmountTxt.text = _goldAmount.ToString();
        if (_goldAmount <= 0)
            _stateMachine.setEvent((int)Events.Finished);
    } 

    Vector3 mouseToWorld()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            Debug.Log("hit");
            Vector3 pos = new Vector3((int)hit.point.x, 0, (int)hit.point.z);
            return pos;
        }

        return new Vector3(-1, -1, -1);
    }
}
