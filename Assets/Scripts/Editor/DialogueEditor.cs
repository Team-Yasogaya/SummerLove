using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace NoName.Editor
{
    public class DialogueEditor : EditorWindow
    {
        public Dialogue SelectedDialogue;
        private GUIStyle _nodeStyle;
        private GUIStyle _playerNodeStyle;
        private GUIStyle _rootNodeStyle;
        private GUIStyle _textAreaStyle;

        // EVENTS SECTION
        [NonSerialized]
        private DialogueNode _draggingNode;
        [NonSerialized]
        private Vector2 _draggingOffset;

        private Vector2 _scrollPosition;

        [NonSerialized]
        private bool _draggingCanvas;
        [NonSerialized]
        private Vector2 _draggingCanvasOffset;

        [NonSerialized]
        private DialogueNode _creatingNode;
        [NonSerialized]
        private DialogueNode _deletingNode;
        [NonSerialized]
        private DialogueNode _linkingNode;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;

            if (dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }

            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;

            _nodeStyle = new GUIStyle();
            _nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            _nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            _nodeStyle.border = new RectOffset(12, 12, 12,12);

            _playerNodeStyle = new GUIStyle();
            _playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            _playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            _playerNodeStyle.border = new RectOffset(12, 12, 12, 12);

            _rootNodeStyle = new GUIStyle();
            _rootNodeStyle.normal.background = EditorGUIUtility.Load("node2") as Texture2D;
            _rootNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            _rootNodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        private void OnSelectionChanged()
        {
            Dialogue trySelectedDialogue = Selection.activeObject as Dialogue;

            if (trySelectedDialogue != null)
            {
                SelectedDialogue = trySelectedDialogue;
                Repaint();
            }
        }

        private void OnGUI()
        {
            if (SelectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected");
            }
            else
            {
                ProcessEvents();

                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

                Rect canvas = GUILayoutUtility.GetRect(4000, 4000);
                Texture2D backgroundTexture = Resources.Load("background") as Texture2D;
                Rect textureCoords = new Rect(0, 0, canvas.width / 50, canvas.height / 50);
                GUI.DrawTextureWithTexCoords(canvas, backgroundTexture, textureCoords);

                foreach (var node in SelectedDialogue.Nodes)
                {
                    DrawConnections(node);
                }

                foreach (var node in SelectedDialogue.Nodes)
                {
                    DrawNode(node);
                }

                EditorGUILayout.EndScrollView();

                if (_creatingNode != null)
                {
                    SelectedDialogue.CreateNode(_creatingNode);
                    _creatingNode = null;
                }

                if (_deletingNode != null)
                {
                    SelectedDialogue.DeleteNode(_deletingNode);
                    _deletingNode = null;
                }
            }
        }

        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && _draggingNode == null) 
            {
                Vector2 clickPosition = Event.current.mousePosition + _scrollPosition; 
                _draggingNode = GetNodeAtPoint(clickPosition);

                if (_draggingNode != null)
                {
                    Undo.RecordObject(_draggingNode, "Move Dialogue Node");
                    _draggingOffset = _draggingNode.Rect.position - Event.current.mousePosition;
                    Selection.activeObject = _draggingNode;
                }
                else
                {
                    _draggingCanvas = true;
                    _draggingCanvasOffset = clickPosition;
                    Selection.activeObject = SelectedDialogue;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && _draggingNode != null)
            {
                _draggingNode.Rect.position = Event.current.mousePosition + _draggingOffset;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && _draggingCanvas == true)
            {
                _scrollPosition = _draggingCanvasOffset - Event.current.mousePosition;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && _draggingNode != null)
            {
                _draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseUp && _draggingCanvas != true)
            {
                _draggingCanvas = false;
            }

        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode returnNode = null;

            foreach (var node in SelectedDialogue.Nodes)
            {
                if (node.Rect.Contains(point))
                {
                    returnNode = node;
                }
            }

            return returnNode;
        }

        private void DrawNode(DialogueNode node)
        {
            if (node.IsPlayerSpeaking)
            {
                GUILayout.BeginArea(node.Rect, _playerNodeStyle);
            }
            else if (SelectedDialogue.RootNode == node)
            {
                GUILayout.BeginArea(node.Rect, _rootNodeStyle);
            }
            else
            {
                GUILayout.BeginArea(node.Rect, _nodeStyle);
            }

            EditorGUILayout.LabelField("Node: ");

            EditorGUI.BeginChangeCheck();

            if (_textAreaStyle == null)
            {
                _textAreaStyle = new GUIStyle(EditorStyles.textArea);
                _textAreaStyle.wordWrap = true;
            }

            string fieldText = EditorGUILayout.TextArea(node.Text, _textAreaStyle, GUILayout.ExpandHeight(true));

            if (EditorGUI.EndChangeCheck())
            {
                node.UpdateText(fieldText);
            }

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("+"))
            {
                _creatingNode = node;
            }

            DrawLinkingButton(node);

            if (GUILayout.Button("-"))
            {
                _deletingNode = node;
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void DrawLinkingButton(DialogueNode node)
        {
            if (_linkingNode == null)
            {
                if (GUILayout.Button("Link"))
                {
                    _linkingNode = node;
                }
            }
            else
            {
                if (_linkingNode == node)
                {
                    if (GUILayout.Button("Cancel"))
                    {
                        _linkingNode = null;
                    }
                }
                else if (_linkingNode.Children.Contains(node.name))
                {
                    if (GUILayout.Button("Unlink"))
                    {
                        _linkingNode.RemoveChild(node);
                        _linkingNode = null;
                    }
                }
                else if (node.Children.Contains(_linkingNode.name))
                {
                    GUILayout.Button("//");
                }
                else
                {
                    if (GUILayout.Button("Child"))
                    {
                        _linkingNode.AddChild(node);
                        _linkingNode = null;
                    }
                }
            }
        }

        private void DrawConnections(DialogueNode node)
        {
            Vector2 startPosition = new Vector2(node.Rect.xMax, node.Rect.center.y);

            foreach (var childNode in SelectedDialogue.ChildrenNodes(node)) 
            {
                Vector2 endPosition = new Vector2(childNode.Rect.xMin, childNode.Rect.center.y);
                Vector2 controlPointsOffset = new Vector2((endPosition.x - startPosition.x) * .8f, 0);

                Handles.DrawBezier(
                    startPosition,
                    endPosition,
                    startPosition + controlPointsOffset,
                    endPosition - controlPointsOffset,
                    Color.white,
                    null,
                    3f
                );
            }
        }
    }
}