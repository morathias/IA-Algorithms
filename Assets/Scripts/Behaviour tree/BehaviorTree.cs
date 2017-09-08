using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType {
    Sequencer,
    Selector,
    LogicAnd,
    LogicOr,
    DecoratorInverter,
    Conditional,
    Action
}

public class BehaviorTree : MonoBehaviour {

    BHNode _root;
    public NodeType _rootType;
	
	void Start () {
        switch (_rootType)
        {
            case NodeType.Sequencer:
                _root = new BHSequencer();
                break;
            case NodeType.Selector:
                _root = new BHSelector();
                break;
            case NodeType.LogicAnd:
                _root = new BHLogicAnd();
                break;
            case NodeType.LogicOr:
                _root = new BHLogicOr();
                break;
            case NodeType.DecoratorInverter:
                _root = new BHDecoratorInverter();
                break;
            case NodeType.Conditional:
                _root = new BHConditional();
                break;
            case NodeType.Action:
                _root = new BHAction();
                break;
            default:
                break;
        }

        BHSelector BHSelector = new BHSelector();
        _root.addChild(BHSelector);

        BHDecoratorInverter BHDecoratorInverter = new BHDecoratorInverter();
        _root.addChild(BHDecoratorInverter);

        BHAction BHAction = new BHAction();
        _root.getChild(0).addChild(BHAction);

        BHLogicAnd BHLogicAnd = new BHLogicAnd();
        _root.getChild(1).addChild(BHLogicAnd);

        _root.displayTree();
	}
	
	void Update () {
		
	}
}
