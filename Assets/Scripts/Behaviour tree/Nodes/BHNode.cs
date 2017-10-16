using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BHNodeState{
    OK,
    EXECUTING,
    ERROR
}

public class BHNode {
    protected List<BHNode> _childs;
    BHNode _parent;

    protected string _type;

    public delegate BHNodeState NodeFunction();
    protected NodeFunction _nodeFunction;

    public BHNode() {
        _childs = new List<BHNode>();
    }

    public bool addChild(BHNode node) {
        if (canHaveChilds()) {
            _childs.Add(node);
            node.setParent(this);
            return true;
        }

        return false;
    }

    public virtual BHNodeState start() { return _nodeFunction(); }

    public BHNode getChild(int index) {
        if(index >= 0 && index <= _childs.Count - 1)
            return _childs[index];

        return null;
    }

    protected virtual bool canHaveChilds() { return true; }

    public void displayTree() {
        Debug.Log(_type);

        for (int i = 0; i < _childs.Count; i++)
            _childs[i].displayTree();
    }

    public void setFunctionToExecute(NodeFunction function) {
        _nodeFunction = function;
    }

    public void setParent(BHNode parent) {
        _parent = parent;
    }
}
