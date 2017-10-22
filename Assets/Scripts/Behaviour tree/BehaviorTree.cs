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

    public BehaviourTreeComponent behaviourTreeComponent;

    public void BHSetup<T>(T treeOwner) {
        behaviourTreeComponent.loadTree<T>(0, out _root, treeOwner);
    }

    public BHNode getRoot() {
        return _root;
    }
}
