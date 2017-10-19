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

    GameObject _goldBag;
    int _goldAmount = 0;
    public Text goldAmountTxt;

    BehaviorTree _behaviourTree;

    public Animator _animations;
	
	protected override void Start () {
        base.Start();
        _goldBag = transform.GetChild(1).gameObject;
        
        _stateMachine = GetComponent<FSM>();
        _stateMachine.init((int)States.StatesCount, (int)Events.EventsCount);

        _state = (States)_stateMachine.getState();

        _stateMachine.setRelation((int)States.Idle, (int)Events.StartMoving, (int)States.Moving);
        _stateMachine.setRelation((int)States.Moving, (int)Events.ReachedPosition, (int)States.Idle);
        _stateMachine.setRelation((int)States.Moving, (int)Events.ReachedCastle, (int)States.Deploying);
        _stateMachine.setRelation((int)States.Deploying, (int)Events.Finished, (int)States.Idle);
        _stateMachine.setRelation((int)States.Moving, (int)Events.ReachedMine, (int)States.Mining);
        _stateMachine.setRelation((int)States.Mining, (int)Events.Finished, (int)States.Idle);

        _behaviourTree = GetComponent<BehaviorTree>();

        _behaviourTree.getRoot().getChild(0).getChild(0).setFunctionToExecute(isIddling);
        _behaviourTree.getRoot().getChild(1).getChild(0).setFunctionToExecute(moving);
        _behaviourTree.getRoot().getChild(1).getChild(1).getChild(0).getChild(0).setFunctionToExecute(isMine);
        _behaviourTree.getRoot().getChild(1).getChild(1).getChild(0).getChild(1).setFunctionToExecute(mining);
        _behaviourTree.getRoot().getChild(1).getChild(1).getChild(1).getChild(0).setFunctionToExecute(isCastle);
        _behaviourTree.getRoot().getChild(1).getChild(1).getChild(1).getChild(1).setFunctionToExecute(hasGold);
        _behaviourTree.getRoot().getChild(1).getChild(1).getChild(1).getChild(2).setFunctionToExecute(deploying);
	}

    protected override void Update()
    {
        if(_state == States.Idle)
            _animations.Play("Miner_iddle");
        if(_state == States.Moving)
            _animations.Play("Miner_walking");
        _animations.SetLookAtPosition(_currentPositionToMove + new Vector3(0.5f, 0f, 0.5f));
    }

    void LateUpdate() {
        _behaviourTree.getRoot().start();
    }

    void stateMachine() {
        _state = (States)_stateMachine.getState();

        switch (_state)
        {
            case States.Idle:
                isIddling();
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

    BHNodeState isIddling() {
        _state = States.Idle;
        

        if (Input.GetMouseButtonDown(1))
        {
            _path = grid.startAlgorithm(transform.position, mouseToWorld());
            _currentPositionToMove = _path.Pop();
            _startMoving = true;
            goal.transform.position = mouseToWorld();

            goalNode = grid.getNode((int)goal.transform.position.x, (int)goal.transform.position.z);

            _stateMachine.setEvent((int)Events.StartMoving);
            return BHNodeState.ERROR;
        }

        return BHNodeState.OK;
    }

    BHNodeState moving() {
        _state = States.Moving;

        _goldBag.transform.rotation = _mesh.transform.rotation;

        Debug.Log("moving");
        base.Update();
        if (transform.position == _currentPositionToMove)
            _path.Clear();

        if (!_startMoving)
        {
            Debug.Log("reached position");
            return BHNodeState.OK;
        }
            
        return BHNodeState.EXECUTING;
    }

    BHNodeState isMine() {
        if (goalNode.getScore() >= 100 && goalNode.getScore() <= 1000)
            return BHNodeState.OK;

        return BHNodeState.ERROR;
    }

    BHNodeState mining()
    {
        _goldAmount++;
        goldAmountTxt.text = _goldAmount.ToString();

        _mesh.gameObject.SetActive(false);

        if (_goldAmount >= 50)
        {
            _stateMachine.setEvent((int)Events.Finished);

            _mesh.gameObject.SetActive(true);
            _goldBag.SetActive(true);

            return BHNodeState.OK;
        }

        return BHNodeState.EXECUTING;
    }

    BHNodeState isCastle() {
        if (goalNode.getScore() >= 1000)
            return BHNodeState.OK;

        return BHNodeState.ERROR;
    }

    BHNodeState hasGold() {
        if (_goldAmount > 0)
            return BHNodeState.OK;

        return BHNodeState.ERROR;
    }

    BHNodeState deploying()
    {
        _goldAmount--;
        goldAmountTxt.text = _goldAmount.ToString();

        if (_goldAmount <= 0)
        {
            _stateMachine.setEvent((int)Events.Finished);
            _goldBag.SetActive(false);
            return BHNodeState.OK;
        }

        return BHNodeState.EXECUTING;
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
