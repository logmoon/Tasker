using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioSettings))]
public class AudioSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(20);
        if (GUILayout.Button("Update Audio Names"))
        {
            AudioSettings.Get().UpdateAudioNames();
        }
    }
}

public class AudioSystemToolbar
{
    [MenuItem("Tools/Audio System/Open Audio Settings")]
    public static void OpenAudioSystemSettings()
    {
        Selection.activeInstanceID = AudioSettings.Get().GetInstanceID();
    }
}

