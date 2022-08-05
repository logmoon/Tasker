using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "Audio Settings", menuName = "Audio System/Audio Settings")]
public class AudioSettings : ScriptableObject
{
    private static AudioSettings Instance;
    [Header("Data")]
    public List<AudioManager.AudioTrack> audioTracks;

    [Header("Names")]
    public List<string> audioNames;
    [Header("Debugging")]
    public bool debugSystem;



# if UNITY_EDITOR
    public void UpdateAudioNames()
    {
        List<string> sounds = new List<string>();
        sounds.Add("None");
        sounds.AddRange(audioNames);
        EnumGenerator.Generate("AudioType", sounds);
    }
# endif


    public static AudioSettings Get()
    {
        if (Instance != null)
        {
            return Instance;
        }

        var audioSettings = Resources.Load("Audio Settings", typeof(AudioSettings)) as AudioSettings;

#if UNITY_EDITOR
        // In case the settings are not found, we create one
        if (audioSettings == null)
        {
            return CreateFile();
        }
#endif

        // In case it still doesn't exist, somehow it got removed.
        if (audioSettings == null)
        {
            Debug.LogWarning("Could not find AudioPluginsSettings in resource folder, did you remove it? Using default settings.");
            audioSettings = ScriptableObject.CreateInstance<AudioSettings>();
        }

        Instance = audioSettings;

        return Instance;
    }

#if UNITY_EDITOR

    public static AudioSettings CreateFile()
    {
        string resourceFolderPath = string.Format("{0}/{1}", Application.dataPath, "Resources");
        string filePath = string.Format("{0}/{1}", resourceFolderPath, "Audio Settings.asset");

        // In case the directory doesn't exist, we create a new one.
        if (!Directory.Exists(resourceFolderPath))
        {
            UnityEditor.AssetDatabase.CreateFolder("Assets", "Resources");
        }

        // Check if the settings file exists in the resources path
        // If not, we create a new one.
        if (!File.Exists(filePath))
        {
            Instance = ScriptableObject.CreateInstance<AudioSettings>();
            UnityEditor.AssetDatabase.CreateAsset(Instance, "Assets/Resources/Audio Settings.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

            return Instance;
        }
        else
        {
            return Resources.Load("Audio Settings", typeof(AudioSettings)) as AudioSettings;
        }
    }
# endif

}
