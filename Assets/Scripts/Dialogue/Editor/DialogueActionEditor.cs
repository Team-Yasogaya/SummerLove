using NoName.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NoName
{
    [CustomPropertyDrawer(typeof(DialogueAction))]
    public class DialogueActionEditor : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var actionType = property.FindPropertyRelative("_actionType").enumValueIndex;
            float baseHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 4f;

            switch (actionType)
            {
                case (int)DialogueAction.ActionType.RemoveItem:
                    return baseHeight * 3;

                case (int)DialogueAction.ActionType.AddItem:
                    return baseHeight * 3; 

                case (int)DialogueAction.ActionType.Custom:
                    return baseHeight * 2; 

                default:
                    return baseHeight;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var actionType = property.FindPropertyRelative("_actionType");
            GUIContent labelContent = new GUIContent("Action");

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), labelContent);



            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            switch (actionType.intValue)
            {
                case (int)DialogueAction.ActionType.RemoveItem:
                case (int)DialogueAction.ActionType.AddItem:
                    Rect typeRect = new Rect(position.x, position.y + 2, position.width, position.height / 3);
                    EditorGUI.PropertyField(typeRect, actionType, GUIContent.none);

                    var itemIdProperty = property.FindPropertyRelative("_itemId");
                    var itemQuantityProperty = property.FindPropertyRelative("_quantity");

                    Rect itemRect = typeRect;
                    itemRect.y += typeRect.height;

                    ItemSO[] allItems = Resources.LoadAll<ItemSO>("");
                    string[] allItemsNames = allItems.Select(item => item.Name).ToArray();

                    int currentIndex = -1;

                    for (int i = 0; i < allItems.Length; i++)
                    {
                        if (allItems[i].Id == itemIdProperty.stringValue) currentIndex = i;
                    }

                    GUIContent itemLabel = new GUIContent("Item");
                    EditorGUI.BeginChangeCheck();
                    int choiceIndex = EditorGUI.Popup(itemRect, currentIndex, allItemsNames);
                    if (EditorGUI.EndChangeCheck())
                    {
                        itemIdProperty.stringValue = allItems[choiceIndex].Id;
                    }

                    GUIContent quantityLabel = new GUIContent("Quantity");
                    Rect quantityRect = itemRect;
                    quantityRect.y += itemRect.height;
                    quantityRect.height -= 4;

                    EditorGUI.BeginChangeCheck();
                    itemQuantityProperty.intValue = EditorGUI.IntField(quantityRect, quantityLabel, itemQuantityProperty.intValue);

                    if (itemQuantityProperty.intValue < 1)
                    {
                        itemQuantityProperty.intValue = 1;
                    }
                    EditorGUI.EndChangeCheck();
                    break;

                case (int)DialogueAction.ActionType.Custom:
                    var customActionProperty = property.FindPropertyRelative("_actionName");

                    typeRect = new Rect(position.x, position.y + 2, position.width, position.height / 2);
                    EditorGUI.PropertyField(typeRect, actionType, GUIContent.none);

                    Rect customActionRect = typeRect;
                    customActionRect.y += typeRect.height;
                    customActionRect.height -= 4;

                    GUIContent customActionLabel = new GUIContent("Action Name");
                    EditorGUI.PropertyField(customActionRect, customActionProperty, customActionLabel);
                    break;

                default:
                    break;
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}
