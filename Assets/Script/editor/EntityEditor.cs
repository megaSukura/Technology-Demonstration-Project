
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(GameEntity))]
public class EntityEditor : Editor
{
    private GameEntity entity;
    private void OnEnable() {
        entity= (GameEntity) target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Separator();
        if(entity.EntityLogicSubhandler!=null)
        {
            GUILayout.Label("LogicSubs");
            EditorGUILayout.BeginVertical("box");
            if(entity.EntityLogicSubhandler.logicSubs.Count==0)
                EditorGUILayout.LabelField("No LogicSubs");
            foreach (var logicSub  in entity.EntityLogicSubhandler.logicSubs)
            {
                if(logicSub!=null)
                {
                    EditorGUILayout.LabelField(logicSub.GetType().ToString());
                    logicSub.OndrawInspector();
                }
                //分割线
                EditorGUILayout.Separator();
            }
            EditorGUILayout.EndVertical();
        }
    }
}
