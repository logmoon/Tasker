using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "Menu Settings", menuName = "Systems/Menu System/Menu Settings")]
public class MenuSettings : ScriptableObject
{
    private static MenuSettings Instance;
    [Header("Data")]
    public List<string> menuNames;

    [Header("Debugging")]
    public bool debugSystem;



# if UNITY_EDITOR
    public void UpdateMenus()
    {
        List<string> menus = new List<string>();
        menus.Add("None");
        menus.AddRange(menuNames);
        EnumGenerator.Generate("MenuIndexes", menus);
    }
# endif


    public static MenuSettings Get()
    {
        if (Instance != null)
        {
            return Instance;
        }

        var menuSettings = Resources.Load("Menu Settings", typeof(MenuSettings)) as MenuSettings;

#if UNITY_EDITOR
        // In case the settings are not found, we create one
        if (menuSettings == null)
        {
            return CreateFile();
        }
#endif

        // In case it still doesn't exist, somehow it got removed.
        if (menuSettings == null)
        {
            Debug.LogWarning("Could not find MenuPluginsSettings in resource folder, did you remove it? Using default settings.");
            menuSettings = ScriptableObject.CreateInstance<MenuSettings>();
        }

        Instance = menuSettings;

        return Instance;
    }

#if UNITY_EDITOR

    public static MenuSettings CreateFile()
    {
        string resourceFolderPath = string.Format("{0}/{1}", Application.dataPath, "Resources");
        string filePath = string.Format("{0}/{1}", resourceFolderPath, "Menu Settings.asset");

        // In case the directory doesn't exist, we create a new one.
        if (!Directory.Exists(resourceFolderPath))
        {
            UnityEditor.AssetDatabase.CreateFolder("Assets", "Resources");
        }

        // Check if the settings file exists in the resources path
        // If not, we create a new one.
        if (!File.Exists(filePath))
        {
            Instance = ScriptableObject.CreateInstance<MenuSettings>();
            UnityEditor.AssetDatabase.CreateAsset(Instance, "Assets/Resources/Menu Settings.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

            return Instance;
        }
        else
        {
            return Resources.Load("Menu Settings", typeof(MenuSettings)) as MenuSettings;
        }
    }
# endif


}
