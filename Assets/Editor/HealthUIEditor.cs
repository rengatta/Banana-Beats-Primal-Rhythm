using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HealthUI))]
public class customButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HealthUI myScript = (HealthUI)target;
        if (GUILayout.Button("Apply Changes"))
        {
            myScript.ValidateFunction();
        }
    }

}