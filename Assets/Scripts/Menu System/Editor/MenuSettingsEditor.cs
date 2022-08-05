using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MenuSettings))]
public class MenuSettingsEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(20);
        if (GUILayout.Button("Update Menu Indexes"))
        {
            MenuSettings.Get().UpdateMenus();
        }
    }
}

public class MenuSystemToolbar
{
    [MenuItem("Tools/Menu System/Open Menu Settings")]
    public static void OpenMenuSystemSettings()
    {
        Selection.activeInstanceID = MenuSettings.Get().GetInstanceID();
    }
}


