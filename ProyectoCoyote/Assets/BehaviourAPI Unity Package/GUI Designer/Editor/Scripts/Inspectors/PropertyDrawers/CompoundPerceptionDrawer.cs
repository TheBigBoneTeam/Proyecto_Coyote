using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace BehaviourAPI.UnityToolkit.GUIDesigner.Editor
{
    using Framework;
    using Core.Perceptions;
    using System;

    [CustomPropertyDrawer(typeof(CompoundPerceptionWrapper))]
    public class CompoundPerceptionPropertyDrawer : PropertyDrawer
    {
        private static readonly float k_RemoveGraphBtnWidth = 40;
        private static readonly float k_SpaceWidth = 10;
        Vector2 _scrollPos;

        private void AddSubPerception(SerializedProperty arrayProperty, System.Type perceptionType)
        {
            arrayProperty.arraySize++;
            var lastElementProperty = arrayProperty.GetArrayElementAtIndex(arrayProperty.arraySize - 1).FindPropertyRelative("perception");

            if (perceptionType.IsSubclassOf(typeof(CompoundPerception)))
            {
                var compound = (CompoundPerception)System.Activator.CreateInstance(perceptionType);
                lastElementProperty.managedReferenceValue = new CompoundPerceptionWrapper(compound);
            }
            else
            {
                lastElementProperty.managedReferenceValue = (Perception)System.Activator.CreateInstance(perceptionType);
            }

            arrayProperty.serializedObject.ApplyModifiedProperties();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.managedReferenceValue == null) return;

            var compoundPerceptionProperty = property.FindPropertyRelative("compoundPerception");

            var subPerceptionProperty = property.FindPropertyRelative("subPerceptions");

            var labelRect = new Rect(position.x, position.y, position.width - (k_RemoveGraphBtnWidth + k_SpaceWidth), position.height);
            var removeRect = new Rect(position.x + position.width - k_RemoveGraphBtnWidth, position.y, k_RemoveGraphBtnWidth, position.height);
            EditorGUI.LabelField(labelRect, compoundPerceptionProperty.managedReferenceValue.TypeName());

            if (GUI.Button(removeRect, "X"))
            {
                property.managedReferenceValue = null;
                property.serializedObject.ApplyModifiedProperties();
                return;
            }

            int deep = compoundPerceptionProperty.propertyPath.Count(c => c == '.');
            foreach (SerializedProperty p in compoundPerceptionProperty)
            {
                if (p.propertyPath.Count(c => c == '.') == deep + 1)
                {
                    EditorGUILayout.PropertyField(p, true);
                }
            }

            if (GUILayout.Button("Add element", EditorStyles.popup))
            {
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
                    ElementCreatorWindowProvider.Create<PerceptionCreationWindow>((pType) => AddSubPerception(subPerceptionProperty, pType)));
            }

            GUIStyle centeredLabelstyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };

            EditorGUILayout.LabelField("Sub perceptions", centeredLabelstyle);
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, "window", GUILayout.MinHeight(300));

            if (subPerceptionProperty != null)
            {
                for (int i = 0; i < subPerceptionProperty.arraySize; i++)
                {
                    var subperception = subPerceptionProperty.GetArrayElementAtIndex(i);
                    var p = subperception.FindPropertyRelative("perception");

                    EditorGUILayout.PropertyField(p);
                    if (GUILayout.Button("Remove"))
                    {
                        subPerceptionProperty.DeleteArrayElementAtIndex(i);
                        property.serializedObject.ApplyModifiedProperties();
                        break;
                    }
                    EditorGUILayout.Space(5);
                }
            }

            EditorGUILayout.EndScrollView();
        }
    }
    [CustomPropertyDrawer(typeof(OppositePerception))]
    public class OppositePerceptionPropertyDrawer : PropertyDrawer
    {
        private static readonly float k_RemoveGraphBtnWidth = 40;
        private static readonly float k_SpaceWidth = 10;
        Vector2 _scrollPos;
        private void AssignPerception(SerializedProperty property, Type perceptionType)
        {

            if (perceptionType.IsSubclassOf(typeof(CompoundPerception)))
            {
                var compound = (CompoundPerception)Activator.CreateInstance(perceptionType);
                property.managedReferenceValue = new CompoundPerceptionWrapper(compound);
            }
            else
            {
                property.managedReferenceValue = (Perception)Activator.CreateInstance(perceptionType);
            }
            property.serializedObject.ApplyModifiedProperties();
        }
        private void AssignSubPerception(SerializedProperty property, Type perceptionType)
        {
            Debug.Log(property.managedReferenceValue == null);
          
            var prop = property.FindPropertyRelative("perception");
            if (perceptionType.IsSubclassOf(typeof(CompoundPerception)))
            {
                var compound = (CompoundPerception)Activator.CreateInstance(perceptionType);
                prop.managedReferenceValue = new CompoundPerceptionWrapper(compound);
            }
            else
            {
                prop.managedReferenceValue = (Perception)Activator.CreateInstance(perceptionType);
            }
            prop.serializedObject.ApplyModifiedProperties();
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.managedReferenceValue == null) return;

            if (property.managedReferenceValue == null)
            {
                if (GUILayout.Button("Assign perception"))
                {
                    SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), ElementCreatorWindowProvider.Create<PerceptionCreationWindow>((pType) => AssignPerception(property, pType)));
                }
            }
            else
            {
                var prop = property.FindPropertyRelative("perception");

                if (prop.managedReferenceValue == null)
                {
                    if (GUILayout.Button("Assign perception"))
                    {
                        SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), ElementCreatorWindowProvider.Create<PerceptionCreationWindow>((pType) => AssignSubPerception(property, pType)));
                        
                    }
                }

               // EditorGUILayout.PropertyField(prop);
                var labelRect = new Rect(position.x, position.y, position.width - (k_RemoveGraphBtnWidth + k_SpaceWidth), position.height);
                var removeRect = new Rect(position.x + position.width - k_RemoveGraphBtnWidth, position.y, k_RemoveGraphBtnWidth, position.height);
                EditorGUI.LabelField(labelRect, property.managedReferenceValue.TypeName());
                if (GUI.Button(removeRect, "X"))
                {
                    property.managedReferenceValue = null;
                }
                else
                {
                    int deep = property.propertyPath.Count(c => c == '.');
                    foreach (SerializedProperty p in property)
                    {
                        if (p.propertyPath.Count(c => c == '.') == deep + 1)
                        {
                            EditorGUILayout.PropertyField(p, true);
                        }
                    }
                }
                prop.serializedObject.ApplyModifiedProperties();
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
