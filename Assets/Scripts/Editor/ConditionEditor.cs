using NoName.Inventory;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NoName
{
    [CustomPropertyDrawer(typeof(Condition))]
    public class ConditionEditor : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var actionType = property.FindPropertyRelative("_conditionType").enumValueIndex;
            float baseHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 4f;

            switch (actionType)
            {
                case (int)Condition.ConditionType.None:
                    return baseHeight;

                case (int)Condition.ConditionType.HasItem:
                    return baseHeight * 3;

                case (int)Condition.ConditionType.Custom:
                    return baseHeight * 3;

                default:
                    return baseHeight;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var customActionProperty = property.FindPropertyRelative("_predicate");
            var actionParametersProperty = property.FindPropertyRelative("_parameters");

            var conditionType = property.FindPropertyRelative("_conditionType");
            GUIContent labelContent = new GUIContent("Condition");

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), labelContent);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            switch (conditionType.intValue)
            {
                case (int)Condition.ConditionType.None:
                    Rect typeRect = new Rect(position.x, position.y + 2, position.width, position.height);
                    EditorGUI.PropertyField(typeRect, conditionType, GUIContent.none);
                    break;

                case (int)Condition.ConditionType.HasItem:
                    typeRect = new Rect(position.x, position.y + 2, position.width, position.height / 3);
                    EditorGUI.PropertyField(typeRect, conditionType, GUIContent.none);

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

                    EditorGUI.BeginChangeCheck();
                    int choiceIndex = EditorGUI.Popup(itemRect, currentIndex, allItemsNames);
                    if (EditorGUI.EndChangeCheck())
                    {
                        actionParametersProperty.arraySize = 2;
                        itemIdProperty.stringValue = allItems[choiceIndex].Id;
                        customActionProperty.stringValue = "HasInventoryItem";

                        var firstParameter = actionParametersProperty.GetArrayElementAtIndex(0);
                        firstParameter.stringValue = itemIdProperty.stringValue;
                    }

                    Rect quantityRect = itemRect;
                    quantityRect.y += itemRect.height;
                    quantityRect.height -= 4;

                    EditorGUI.BeginChangeCheck();
                    itemQuantityProperty.intValue = EditorGUI.IntField(quantityRect, itemQuantityProperty.intValue);

                    if (itemQuantityProperty.intValue < 1)
                    {
                        itemQuantityProperty.intValue = 1;

                        if (actionParametersProperty.arraySize < 2)
                        {
                            actionParametersProperty.arraySize = 2;
                        }

                        var secondParameter = actionParametersProperty.GetArrayElementAtIndex(1);

                        if (secondParameter != null)
                        {
                            secondParameter.stringValue = itemQuantityProperty.intValue.ToString();
                        }
                    }
                    EditorGUI.EndChangeCheck();
                    break;

                case (int)Condition.ConditionType.Custom:

                    typeRect = new Rect(position.x, position.y + 2, position.width, position.height / 3);
                    EditorGUI.PropertyField(typeRect, conditionType, GUIContent.none);

                    Rect customActionRect = typeRect;
                    customActionRect.y += typeRect.height;
                    customActionRect.height -= 4;

                    GUIContent customActionLabel = new GUIContent("Predicate");
                    EditorGUI.PropertyField(customActionRect, customActionProperty, GUIContent.none);
                    break;

                default:
                    break;
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}
