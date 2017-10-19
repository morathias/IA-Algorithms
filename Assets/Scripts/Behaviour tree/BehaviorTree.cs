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

    public BehaviourTreeComponent behaviourTreeComponent;
	
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
        //first layer
        BHDecoratorInverter BHDecoratorInverter = new BHDecoratorInverter();
        _root.addChild(BHDecoratorInverter);

        BHConditional bhConditionalIsIddling = new BHConditional();
        _root.getChild(0).addChild(bhConditionalIsIddling);

        BHSequencer bhSequencerMoving = new BHSequencer();
        _root.addChild(bhSequencerMoving);
        //second layer
        BHAction bhActionMoving = new BHAction();
        _root.getChild(1).addChild(bhActionMoving);

        BHSelector bhSelectorMiningOrDeploying = new BHSelector();
        _root.getChild(1).addChild(bhSelectorMiningOrDeploying);
        //third layer
        BHSequencer bhSequencerMining = new BHSequencer();
        _root.getChild(1).getChild(1).addChild(bhSequencerMining);

        BHConditional bhConditionalIsMine = new BHConditional();
        _root.getChild(1).getChild(1).getChild(0).addChild(bhConditionalIsMine);

        BHAction bhActionMining = new BHAction();
        _root.getChild(1).getChild(1).getChild(0).addChild(bhActionMining);

        BHSequencer bhSequencerDeploying = new BHSequencer();
        _root.getChild(1).getChild(1).addChild(bhSequencerDeploying);

        BHConditional bhConditionalIsCastle = new BHConditional();
        _root.getChild(1).getChild(1).getChild(1).addChild(bhConditionalIsCastle);

        BHConditional bhConditionalHasGold = new BHConditional();
        _root.getChild(1).getChild(1).getChild(1).addChild(bhConditionalHasGold);

        BHAction bhActionDeploying = new BHAction();
        _root.getChild(1).getChild(1).getChild(1).addChild(bhActionDeploying);

        _root.displayTree();
	}
	
	void Update () {
		
	}

    public BHNode getRoot() {
        return _root;
    }
}
