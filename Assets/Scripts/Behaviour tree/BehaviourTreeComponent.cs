using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class BehaviourTreeComponent : ScriptableObject 
{
    public List<BHNodeSerializable> bhNodesSerializable = new List<BHNodeSerializable>();

    public string scriptPath;

    public int loadTree<T>(int index, out BHNode node, T instanceType) {
        BHNodeSerializable serializableNode = bhNodesSerializable[index];

        MethodInfo function;
        NodeFunction functionToExecute;

        switch ((NodeType)serializableNode.nodeType)
        {
            case NodeType.Sequencer:
                node = new BHSequencer();
                break;
            case NodeType.Selector:
                node = new BHSelector();
                break;
            case NodeType.LogicAnd:
                node = new BHLogicAnd();
                break;
            case NodeType.LogicOr:
                node = new BHLogicOr();
                break;
            case NodeType.DecoratorInverter:
                node = new BHDecoratorInverter();
                break;
            case NodeType.Conditional:
                node = new BHConditional();

                function = instanceType.GetType().GetMethod(serializableNode.functionName, BindingFlags.NonPublic | BindingFlags.Instance);
                if (function != null)
                {
                    Debug.Log("setting function");
                    functionToExecute = (NodeFunction)Delegate.CreateDelegate(typeof(NodeFunction),
                                                                              instanceType,
                                                                              function);
                    node.setFunctionToExecute(functionToExecute);
                }
                break;

            case NodeType.Action:
                node = new BHAction();

                function = instanceType.GetType().GetMethod(serializableNode.functionName, BindingFlags.NonPublic | BindingFlags.Instance);
                if (function != null)
                {
                    Debug.Log("setting function");
                    functionToExecute = (NodeFunction)Delegate.CreateDelegate(typeof(NodeFunction),
                                                                              instanceType,
                                                                              function);
                    node.setFunctionToExecute(functionToExecute);
                }
                break;

            default:
                node = new BHNode();
                break;
        }

        for (int i = 0; i < serializableNode.childCount; i++)
        {
            BHNode childNode;
            index = loadTree<T>(++index, out childNode, instanceType);
            node.addChild(childNode);
        }

        return index;
    }
}
