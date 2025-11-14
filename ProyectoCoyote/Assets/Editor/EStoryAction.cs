using CombatEffect;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(StoryAction))]

public class EStoryAction: PropertyDrawer
{
    SerializedProperty actionType;
    SerializedProperty playOnRestart;

    SerializedProperty nameKey;
    SerializedProperty cutsceneData;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Color backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f); // Dark gray

        // Draw the background rectangle

        actionType = property.FindPropertyRelative("actionType");
        nameKey = property.FindPropertyRelative("nameKey");
        cutsceneData = property.FindPropertyRelative("cutsceneData");
        playOnRestart = property.FindPropertyRelative("playOnRestart");

        EditorGUI.BeginProperty(position, label, property);
        Rect foldOutBox = new Rect(position.min.x, position.min.y, position.size.x, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldOutBox, property.isExpanded, label);
        if (property.isExpanded)
        {
            drawField(actionType, actionType.name);


            //EditorGUILayout.PropertyField(actionType);

            switch ((StoryActionType)actionType.intValue)
            {
                case StoryActionType.changeScene:
                    drawField(nameKey,"sceneName");
                    break;
                case StoryActionType.playDialog:
                    drawField(playOnRestart);

                    drawField(nameKey, "dialogBaseName");
                    break;
                case StoryActionType.continueNonCombatGameplay:

                    break;
                case StoryActionType.startCutscene:

                    drawField(playOnRestart);
                    drawField(cutsceneData, "CutsceneData");

                    break;

            }
        }
        GUILayout.Space(20);

    }

    private void DrawStringProperty(string name)
    {
        drawField(nameKey, name);

    }

    void drawField(SerializedProperty property,string guicontent=null)
    {
        EditorGUILayout.BeginHorizontal();

        GUILayout.Space(10);
        if (guicontent != null)
        {
            EditorGUILayout.PropertyField(property,new GUIContent(guicontent));
        }
        else
        {
            EditorGUILayout.PropertyField(property);

        }
        EditorGUILayout.EndHorizontal();

    }
}
//[CustomPropertyDrawer(typeof( ReactionDataForMultiple))]
//public class EReactionDataForMultiple : PropertyDrawer
//{
//    SerializedProperty animMame;
//    SerializedProperty effects;
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        animMame = property.FindPropertyRelative("animName");
//        effects = property.FindPropertyRelative("effects");

//        ReactionDataForMultiple data = fieldInfo.GetValue(property.serializedObject.targetObject) as ReactionDataForMultiple;

//        EditorGUI.BeginProperty(position, label, property);
//        drawField(animMame);
//        drawField(effects);
//        if (GUILayout.Button("Add Damage"))
//        {
//            //data.effects ??= new List<ACombatEffect>();
//            effects.InsertArrayElementAtIndex(effects.arraySize);

//        }
//        if (GUILayout.Button("Add Stun"))
//        {
//            data.effects ??= new List<ACombatEffect>();

//            data.effects.Add(new StunEffect(null,3));
//        }
//        if (GUILayout.Button("Add Crit Damage"))
//        {
//            data.effects ??= new List<ACombatEffect>();

//            data.effects.Add(new CritDamageEffect(null,3));
//        }
//        GUILayout.Space(20);

//    }
//    void drawField(SerializedProperty property, string guicontent = null)
//    {
//        EditorGUILayout.BeginHorizontal();

//        GUILayout.Space(10);
//        if (guicontent != null)
//        {
//            EditorGUILayout.PropertyField(property, new GUIContent(guicontent));
//        }
//        else
//        {
//            EditorGUILayout.PropertyField(property);

//        }
//        EditorGUILayout.EndHorizontal();

//    }
//}
