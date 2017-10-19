using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class BHEditorNode {
    Rect _rect, _childButtonRect, _parentButtonRect;
    GUIStyle _style, _childButtonStyle, _parentButtonStyle;

    BHEditorNode _parent;
    List<BHEditorNode> _childNodes;

    bool _isDragged;
    bool _isSelected;
    GUIStyle _selectedStyle;

    NodeType _nodeType;

    Texture _nodeIcon;
    Rect _iconRect;
    GUIStyle _iconStyle;

    Rect _parentLabelRect, _childLabelRect;
    GUIStyle _labelStyle;

    public Action<BHEditorNode> removeNode;

    Rect _functionLabelRect;
    Rect _functionInputRect;
    GUIStyle _functionInputStyle;
    string functionName = "name";

    public BHEditorNode(NodeType nodeType, Vector2 position, GUIStyle style, GUIStyle childButtonStyle, GUIStyle parentButtonStyle) {
        _nodeType = nodeType;
        Debug.Log(_nodeType);

        _style = style;
        _selectedStyle = new GUIStyle();
        _selectedStyle.normal.background = _style.active.background;
        _parentButtonStyle = parentButtonStyle;
        _childButtonStyle = childButtonStyle;

        if (_nodeType == NodeType.Action || _nodeType == NodeType.Conditional)
            _rect = new Rect(position.x, position.y, 150f, 90f);
        else
            _rect = new Rect(position.x, position.y, 100f, 90f);

        _childButtonRect = new Rect(_rect.x + _rect.width / 2 - 5, _rect.y + _rect.height - 8, 10f, 10f);
        _childLabelRect = new Rect(_rect.x + _rect.width / 2 - 17, _rect.y + _rect.height - 18, 100f, 10f);

        _parentButtonRect = new Rect(_rect.x + _rect.width / 2 - 5, _rect.y - 5, 10f, 10f);
        _parentLabelRect = new Rect(_rect.x + _rect.width / 2 - 17, _rect.y + 2, 100, 10);

        _labelStyle = new GUIStyle();
        _labelStyle.fontSize = 10;

        _childNodes = new List<BHEditorNode>();

        selectIcon();
        _iconRect = new Rect(_rect.x + _rect.width / 2 - 15, _rect.y + 12, 50, 45);

        _isSelected = false;

        _functionLabelRect = new Rect(_rect.x + 3, _rect.y + _rect.height * 0.60f, 20f, 20f);
        _functionInputRect = new Rect(_functionLabelRect.x + _functionLabelRect.width * 2.5f, _functionLabelRect.y, 130f, 20f);
        _functionInputStyle = new GUIStyle();
        _functionInputStyle.normal.background = _style.normal.background;
        _functionInputStyle.fontSize = 10;
        _functionInputStyle.fixedWidth = 90f;
        _functionInputStyle.fixedHeight = 15f;
    }

    void selectIcon() {
        switch (_nodeType)
        {
            case NodeType.Sequencer:
                _nodeIcon = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Textures/sequencer_icon.png", typeof(Texture));
                break;
            case NodeType.Selector:
                _nodeIcon = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Textures/selector_icon.png", typeof(Texture));
                break;
            case NodeType.LogicAnd:
                _nodeIcon = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Textures/logicand_icon.png", typeof(Texture));
                break;
            case NodeType.LogicOr:
                _nodeIcon = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Textures/logicor_icon.png", typeof(Texture));
                break;
            case NodeType.DecoratorInverter:
                _nodeIcon = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Textures/inverter_icon.png", typeof(Texture));
                break;
            case NodeType.Conditional:
                _nodeIcon = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Textures/conditional_icon.png", typeof(Texture));
                break;
            case NodeType.Action:
                _nodeIcon = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Textures/action_icon.png", typeof(Texture));
                break;
            default:
                break;
        }

        _iconStyle = new GUIStyle();
        _iconStyle.normal.background = (Texture2D)_nodeIcon;
        _iconStyle.fixedWidth = 30;
        _iconStyle.fixedHeight = 25;
    }

    public void draw() 
    {
        if(!_isSelected)
            GUI.Box(_rect, "", _style);
        else
            GUI.Box(_rect, "", _selectedStyle);

        GUI.Box(_iconRect, "", _iconStyle);
        GUI.Label(_parentLabelRect, "parent", _labelStyle);

        if (_nodeType == NodeType.Action || _nodeType == NodeType.Conditional)
        {
            functionName = GUI.TextField(_functionInputRect, functionName, _functionInputStyle);
            GUI.Label(_functionLabelRect, "function:", _labelStyle);
            return;
        }
        GUI.Label(_childLabelRect, "childs", _labelStyle);
    }

    public bool processEvents(Event events) 
    {
        switch (events.type)
        {
            case EventType.ContextClick:
                if (_rect.Contains(events.mousePosition) && _isSelected)
                {
                    showDeleteMenu();
                    events.Use();
                }
                break;

            case EventType.DragExited:
                break;
            case EventType.DragPerform:
                break;
            case EventType.DragUpdated:
                break;
            case EventType.ExecuteCommand:
                break;
            case EventType.Ignore:
                break;

            case EventType.KeyDown:
                if (_isSelected && events.keyCode == KeyCode.Delete)
                    deleteThisNode();
                break;

            case EventType.KeyUp:
                break;
            case EventType.Layout:
                break;

            case EventType.MouseDown:
                if (events.button == 0)
                {
                    if (_rect.Contains(events.mousePosition))
                    {
                        _isDragged = true;
                        GUI.changed = true;
                        _isSelected = true;
                    }

                    else
                    {
                        GUI.changed = true;
                        _isSelected = false;
                    }
                }
                break;

            case EventType.MouseDrag:
                if (events.button == 0 && _isDragged)
                {
                    drag(events.delta);
                    events.Use();
                    return true;
                }
                break;

            case EventType.MouseEnterWindow:
                break;
            case EventType.MouseLeaveWindow:
                break;
            case EventType.MouseMove:
                break;

            case EventType.MouseUp:
                _isDragged = false;
                break;

            case EventType.Repaint:
                break;
            case EventType.ScrollWheel:
                break;
            case EventType.Used:
                break;
            case EventType.ValidateCommand:
                break;
            default:
                break;
        }

        return false;
    }

    void showDeleteMenu() {
        GenericMenu menu = new GenericMenu();

        menu.AddItem(new GUIContent("delete node"), false, deleteThisNode);
        menu.ShowAsContext();
    }

    void deleteThisNode() {
        removeNode(this);

        if(_parent != null)
            _parent.removeChild(this);

        for (int i = 0; i < _childNodes.Count; i++)
            _childNodes[i].clearParent();
    }

    public bool parentButtonPressed() {
        return GUI.Button(_parentButtonRect, "", _parentButtonStyle);
    }

    public bool childButtonPressed() {
        if (_nodeType == NodeType.Action || _nodeType == NodeType.Conditional)
            return false;

        return GUI.Button(_childButtonRect, "", _childButtonStyle);
    }

    public void select(bool isSelected) {
        _isSelected = isSelected;
    }

    public void drag(Vector2 delta) {
        _rect.position += delta;
        _childButtonRect.position += delta;
        _parentButtonRect.position += delta;
        _iconRect.position += delta;
        _parentLabelRect.position += delta;
        _childLabelRect.position += delta;
        _functionInputRect.position += delta;
        _functionLabelRect.position += delta;
    }

    public void addChild(BHEditorNode child) {
        if (!_childNodes.Contains(child))
        {
            if (child != this)
            {
                _childNodes.Add(child);
                Debug.Log("child added");
            }
        }

        else
        {
            removeChild(child);
            _childNodes.Add(child);
            Debug.Log("child added without repeating");
        }
    }

    public void removeChild(BHEditorNode child) 
    {
        _childNodes.Remove(child);
    }

    public void setParent(BHEditorNode parent) 
    {
        if (parent == this)
            return;

        Debug.Log("seting parent");
        if (parent != _parent && _parent != null)
            _parent.removeChild(this);

         _parent = parent;
    }
    public void clearParent() {
        _parent = null;
    }

    public void fillBHNode(BHNode bhNode) {
        for (int i = 0; i < _childNodes.Count; i++)
        {
            BHNode node;
            switch (_childNodes[i].getNodeType())
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
                    break;
                case NodeType.Action:
                    node = new BHAction();
                    break;
                default:
                    node = new BHNode();
                    break;
            }
        }
    }

    public void drawConnectedChilds() 
    {
        if (_childNodes.Count == 0)
            return;

        for (int i = 0; i < _childNodes.Count; i++)
        {
            Vector3[] points = new Vector3[4];

            int[] pointsIndex = {0, 1,
                                 1, 2,
                                 2, 3};

            points[0].x = _rect.x + _rect.width / 2;
            points[0].y = _rect.y + _rect.height;

            points[1].x = _rect.x + _rect.width / 2;
            points[1].y = (points[0].y + _childNodes[i].getRect().y) / 2;

            points[2].x = _childNodes[i].getRect().x + _childNodes[i].getRect().width / 2;
            points[2].y = (points[0].y + _childNodes[i].getRect().y) / 2;

            points[3].x = points[2].x;
            points[3].y = _childNodes[i].getRect().y;

            Handles.color = Color.black;
            Handles.DrawLines(points, pointsIndex);
        }
    }

    public Rect getRect() 
    {
        return _rect;
    }
    public void setRect(Rect rect) 
    {
        _rect = rect;
    }

    public NodeType getNodeType() {
        return _nodeType;
    }
}
