using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

[System.Serializable]
public class BehaviourTreeComponent : ScriptableObject 
{
    BHNode _root;

    XmlDocument _editorState;

    public void setRootNode(BHNode rootNode) {
        _root = rootNode;
    }

    public void saveEditorState(XmlDocument editorState) {
        _editorState = editorState;
    }

    public XmlDocument getEditorState() {
        return _editorState;
    }
}
