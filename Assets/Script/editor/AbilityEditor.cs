using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
[CustomEditor(typeof(AbilityManager))]
public class AbilityEditor :Editor
{
    private AbilityManager abilityManager;
    private void OnEnable() {
        abilityManager= (AbilityManager) target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.BeginVertical("box");
        GUILayout.Label("Ability Manager");

        if(abilityManager.abilities.Count>0)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Ability Name");
             
            GUILayout.EndHorizontal();
            foreach(var ability in abilityManager.abilities)
            {
                GUILayout.BeginVertical("box");
                
                GUILayout.Label(ability.AbilityName+" :");
                EditorGUILayout.Toggle("Active ",ability.isAbilityActive);
                
                //space
                GUILayout.Space(10);
                ability.OndrawInspector();
                GUILayout.EndVertical();
            }
        }
        else
        {
            GUILayout.Label("No Ability");
        }

        GUILayout.EndVertical();
    }
}
