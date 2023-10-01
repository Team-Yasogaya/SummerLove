using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NoName
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private DialogueNode _rootNode;
        [SerializeField] private List<DialogueNode> _nodes = new ();
        [SerializeField] private int _maxCollectableClues;

        private Dictionary<string, DialogueNode> _nodeLookup = new();

        public IEnumerable<DialogueNode> Nodes { get { return _nodes; } }
        public DialogueNode RootNode { get { return _rootNode; } }
        public int MaxCollectableClues { get { return _maxCollectableClues; } }
     

        private void OnValidate()
        {
            _nodeLookup.Clear();

            foreach (var node in _nodes)
            {
                _nodeLookup[node.name] = node;
            }
        }
        
        public IEnumerable<DialogueNode> ChildrenNodes(DialogueNode node)
        {
            foreach (var child in node.Children)
            {
                if (_nodeLookup.TryGetValue(child, out DialogueNode value)) 
                { 
                    yield return value; 
                }
            }
        }

        public DialogueNode GetNode(string nodeID)
        {
            if (_nodeLookup.TryGetValue(nodeID, out DialogueNode node))
            {
                return node;
            }
            else
            {
                Debug.Log("Searched Node doesn't exists");
                return null;
            }
        }

#if UNITY_EDITOR
        public void CreateNode(DialogueNode parent)
        {
            DialogueNode newNode = MakeNode(parent);

            Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");
            Undo.RecordObject(this, "Node Added");

            AddNode(newNode);

            OnValidate();
        }

        private void AddNode(DialogueNode newNode)
        {
            _nodes.Add(newNode);
        }

        private static DialogueNode MakeNode(DialogueNode parent)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.name = System.Guid.NewGuid().ToString();

            if (parent != null)
            {
                newNode.Rect.x = parent.Rect.xMax + 50;
                newNode.Rect.y = parent.Rect.y;
                parent.AddChild(newNode);
            }

            return newNode;
        }

        public void DeleteNode(DialogueNode node)
        {
            Undo.RecordObject(this, "Node Removed");
            _nodes.Remove(node);

            OnValidate();
            CleanDanglingNodes(node);
            Undo.DestroyObjectImmediate(node);
        }

        private void CleanDanglingNodes(DialogueNode deletingNode)
        {
            foreach (var node in _nodes)
            {
                if (node.Children.Contains(deletingNode.name))
                {
                    node.Children.Remove(deletingNode.name);
                }
            }
        }

#endif

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (_nodes.Count == 0)
            {
                DialogueNode newNode = MakeNode(null);
                _rootNode = newNode;

                AddNode(newNode);
            }

            if (AssetDatabase.GetAssetPath(this) == "") return;

            foreach (DialogueNode node in _nodes)
            {
                if (AssetDatabase.GetAssetPath(node) != "") continue;

                AssetDatabase.AddObjectToAsset(node, this);
            }
#endif
        }

        public void OnAfterDeserialize()
        {

        }

    }
}