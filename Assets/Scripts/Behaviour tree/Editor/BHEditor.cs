using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

public static class RectExtensions {
    public static Vector2 topLeft(this Rect rect) {
        return new Vector2(rect.xMin, rect.yMin);
    }

    public static Rect scaleSizeBy(this Rect rect, float scale) {
        return rect.scaleSizeBy(scale, rect.center);
    }

    public static Rect scaleSizeBy(this Rect rect, float scale, Vector2 pivotPoint) {
        Rect result = rect;

        result.x -= pivotPoint.x;
        result.y -= pivotPoint.y;

        result.xMin *= scale;
        result.xMax *= scale;
        result.yMin *= scale;
        result.yMax *= scale;

        result.x += pivotPoint.x;
        result.y += pivotPoint.y;

        return result;
    }
}

public class BHEditor : EditorWindow {
    BHEditorNode root;
    List<BHEditorNode> _nodes = new List<BHEditorNode>();

    BHEditorNode selectedParentNode;
    BHEditorNode selectedChildNode;

    GUIStyle parentButtonStyle;
    GUIStyle childButtonStyle;
    GUIStyle nodeBoxStyle;

    Vector2 offset;
    Vector2 drag;

    float zoomFactor = 1f;

    Matrix4x4 _oldMatrix;
    Vector2 vanishingPoint;

    MonoScript script;

    public BehaviourTreeComponent behaviourTreeComponent;

    Rect bhTreeComponentRect = new Rect(0f, 0f, 250f, 20f);

    [MenuItem("Window/Behaviour tree editor")]
    public static void ShowEditor()
    {
        EditorWindow.GetWindow(typeof(BHEditor));
    }

    void OnEnable() {
        parentButtonStyle = new GUIStyle();
        parentButtonStyle.normal.background = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/parent_button.png", typeof(Texture2D));
        parentButtonStyle.active.background = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/parent_button_pressed.png", typeof(Texture2D));
        parentButtonStyle.fixedHeight = 10;
        parentButtonStyle.fixedWidth = 10;

        childButtonStyle = new GUIStyle();
        childButtonStyle.normal.background = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/child_button.png", typeof(Texture2D));
        childButtonStyle.active.background = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/child_button_pressed.png", typeof(Texture2D));
        childButtonStyle.fixedHeight = 16;
        childButtonStyle.fixedWidth = 12;

        nodeBoxStyle = new GUIStyle();
        nodeBoxStyle.normal.background = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/node_normal.png", typeof(Texture2D));
        nodeBoxStyle.active.background = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/node_selected.png", typeof(Texture2D));
        nodeBoxStyle.fixedHeight = 90f;
        if (behaviourTreeComponent != null)
        {
            loadScript();
            if (behaviourTreeComponent.bhNodesSerializable.Count > 0)
                readBHTreeComponent(0, out root);
        }
    }

    void OnGUI() 
    {
        Event current = Event.current;
        ProcessNodeEvents(current);
        processEvents(current);

        //beginZoomArea();

        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.2f, Color.gray);

        drawNodes();

        drawMenu();
       // endZoomArea();
        if (GUI.changed) Repaint();
    }

    void drawMenu() {
        GUI.Box(new Rect(0f, 0f, 260f, 100f), "");

        EditorGUI.BeginChangeCheck();
        behaviourTreeComponent = (BehaviourTreeComponent)EditorGUI.ObjectField(bhTreeComponentRect,
                                                                               behaviourTreeComponent,
                                                                               typeof(BehaviourTreeComponent));

        if (EditorGUI.EndChangeCheck())
        {
            loadScript();
            _nodes.Clear();

            if (behaviourTreeComponent == null || behaviourTreeComponent.bhNodesSerializable.Count == 0)
                _nodes.Clear();
            else
            {
                if (behaviourTreeComponent.bhNodesSerializable.Count > 0)
                    readBHTreeComponent(0, out root);
            }
        }

        if (behaviourTreeComponent != null)
        {
            EditorGUI.BeginChangeCheck();

            script = (MonoScript)EditorGUI.ObjectField(new Rect(0, 20, 250, 20), script, typeof(MonoScript));

            if (EditorGUI.EndChangeCheck())
                behaviourTreeComponent.scriptPath = AssetDatabase.GetAssetPath(script);
        }

        if (GUI.Button(new Rect(0, 40, 100, 20), "save state"))
            fillBHTreeComponent(script.GetClass());
    }

    void beginZoomArea() {
        GUI.EndGroup();

        Rect zoomedArea = new Rect(0, 0, Screen.width, Screen.height);
        zoomedArea = zoomedArea.scaleSizeBy(1.0f / zoomFactor, vanishingPoint);
        zoomedArea.y += 21f;

        GUI.BeginGroup(zoomedArea);

        _oldMatrix = GUI.matrix;

        Matrix4x4 Translation = Matrix4x4.TRS(zoomedArea.topLeft(), Quaternion.identity, Vector3.one);
        Matrix4x4 Scale = Matrix4x4.Scale(new Vector3(zoomFactor, zoomFactor, 1.0f));
        GUI.matrix = Translation * Scale * Translation.inverse;
    }

    void endZoomArea() {
        GUI.matrix = _oldMatrix;
        GUI.EndGroup();
        GUI.BeginGroup(new Rect(0, 21f,Screen.width, Screen.height));
    }

    void drawNodes() {
        for (int i = 0; i < _nodes.Count; i++)
        {
            _nodes[i].draw();

            _nodes[i].drawConnectedChilds();

            if (_nodes[i].parentButtonPressed()) {
                selectedChildNode = _nodes[i];

                if (selectedParentNode != null)
                    setConnection();
            }

            if (_nodes[i].childButtonPressed()) {
                selectedParentNode = _nodes[i];

                if (selectedChildNode != null)
                    setConnection();
            }
        }
    }

    private void ProcessNodeEvents(Event e)
    {
        if (_nodes != null)
        {
            for (int i = _nodes.Count - 1; i >= 0; i--)
            {
                bool guiChanged = _nodes[i].processEvents(e);

                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }
        }
    }

    void setConnection() {
        selectedParentNode.addChild(selectedChildNode);
        selectedChildNode.setParent(selectedParentNode);

        selectedChildNode = null;
        selectedParentNode = null;

        fillBHTreeComponent(script.GetClass());
    }

    void processEvents(Event events) {
        drag = Vector2.zero;
        

        switch (events.type)
        {
            case EventType.ContextClick:
                if(behaviourTreeComponent != null)
                    showPopUpMenu(events);
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
                break;
            case EventType.KeyUp:
                break;
            case EventType.Layout:
                break;
            case EventType.MouseDown:
                break;

            case EventType.MouseDrag:
                if (events.button == 0) {
                    drag = events.delta;
                    panning(drag);

                    GUI.changed = true;
                }
                break;

            case EventType.MouseEnterWindow:
                break;
            case EventType.MouseLeaveWindow:
                break;
            case EventType.MouseMove:
                break;
            case EventType.MouseUp:
                
                break;
            case EventType.Repaint:
                break;

            case EventType.ScrollWheel:
                vanishingPoint = events.mousePosition;

                GUI.changed = true;

                float zoomDelta = 0.03f;
                zoomDelta = events.delta.y < 0 ? zoomDelta : -zoomDelta;
                zoomFactor += zoomDelta;
                zoomFactor = Mathf.Clamp(zoomFactor, 0.1f, 1f);
                
                events.Use();
                break;

            case EventType.Used:
                break;
            case EventType.ValidateCommand:
                break;
            default:
                break;
        }
    }

    void panning(Vector2 delta) {
        for (int i = 0; i < _nodes.Count; i++)
            _nodes[i].drag(delta);
    }

    void addNodeToMenu(GenericMenu menu, string menuPath, NodeType nodeType, Vector2 position) {
        menu.AddItem(new GUIContent(menuPath), false, () =>addNode(nodeType, position));
    }

    void addNode(NodeType type, Vector2 pos) {
        BHEditorNode node = new BHEditorNode(type, 
                                             pos, 
                                             nodeBoxStyle, 
                                             childButtonStyle, 
                                             parentButtonStyle);

        node.removeNode = removeNode;

        if (type == NodeType.Action || type == NodeType.Conditional)
            node.setScriptType(script.GetClass());

        _nodes.Add(node);
    }

    void showPopUpMenu(Event events) {
        GenericMenu menu = new GenericMenu();

        menu.AddDisabledItem(new GUIContent("Add Node"));

        menu.AddSeparator("");

        addNodeToMenu(menu, "Sequencer", NodeType.Sequencer, events.mousePosition);
        addNodeToMenu(menu, "Selector", NodeType.Selector, events.mousePosition);
        addNodeToMenu(menu, "Conditional", NodeType.Conditional, events.mousePosition);
        addNodeToMenu(menu, "Action", NodeType.Action, events.mousePosition);
        addNodeToMenu(menu, "Decorators/Inverter", NodeType.DecoratorInverter, events.mousePosition);
        addNodeToMenu(menu, "Logic/And", NodeType.LogicAnd, events.mousePosition);
        addNodeToMenu(menu, "Logic/Or", NodeType.LogicOr, events.mousePosition);

        menu.ShowAsContext();
    }

    void removeNode(BHEditorNode node) {
        _nodes.Remove(node);
        fillBHTreeComponent(script.GetClass());
    }

    void fillBHTreeComponent(System.Type scriptType) {
        behaviourTreeComponent.bhNodesSerializable.Clear();
        _nodes[0].fillBHNode(behaviourTreeComponent.bhNodesSerializable);
        
        EditorUtility.SetDirty(behaviourTreeComponent);
    }

    int readBHTreeComponent(int index, out BHEditorNode bhEditorNode) {
        BHNodeSerializable serializableNode = behaviourTreeComponent.bhNodesSerializable[index];

        bhEditorNode = new BHEditorNode((NodeType)serializableNode.nodeType, 
                                                  serializableNode.editorPos, 
                                                  nodeBoxStyle, childButtonStyle, parentButtonStyle);

        bhEditorNode.setFunctionName(serializableNode.functionName);
        bhEditorNode.removeNode = removeNode;
        bhEditorNode.setScriptType(script.GetClass());

        _nodes.Add(bhEditorNode);

        for (int i = 0; i < serializableNode.childCount; i++)
        {
            BHEditorNode childEditorNode;
            index = readBHTreeComponent(++index, out childEditorNode);
            childEditorNode.setParent(bhEditorNode);
            bhEditorNode.addChild(childEditorNode);
        }

        return index;
    }

    void loadScript() {
        if (behaviourTreeComponent == null)
            return;

        if (behaviourTreeComponent.scriptPath.Length > 0)
            script = (MonoScript)AssetDatabase.LoadAssetAtPath<MonoScript>(behaviourTreeComponent.scriptPath);
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.4f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);

        for (int j = 0; j < heightDivs; j++)
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);

        Handles.color = Color.white;
        Handles.EndGUI();
    }
}
