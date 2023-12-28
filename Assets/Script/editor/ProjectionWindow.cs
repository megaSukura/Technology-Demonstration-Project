using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ProjectionWindow : EditorWindow {

    [MenuItem("Technology Demonstration Project/ProjectionWindow")]
    private static void ShowWindow() {
        var window = GetWindow<ProjectionWindow>();
        window.titleContent = new GUIContent("ProjectionWindow");
        window.Show();
    }

    private void OnGUI() {
        //show Projection cont in editor
        if (Projection.DefaultProjection != null) {
            EditorGUILayout.ObjectField("Default Projection", Projection.DefaultProjection, typeof(GameObject), false);
        }
        else {
            EditorGUILayout.LabelField("Default Projection is null");
        }
        //绘制分割线
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        //active Projection in pool
        EditorGUILayout.LabelField("Projection Pool Count: " + Projection.PoolCount);
        EditorGUILayout.LabelField("Projection Pool Active Count: " + Projection.PoolActiveCount);

    }
    void OnInspectorUpdate() {
        this.Repaint();
    }
}