using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public SaveData SaveData { get; private set; }
    public bool FirePlaying { get; set; }
    public bool RainPlaying { get; set; }

    private void Awake()
    {
        // Load save data
        CreateSaveData();
        SaveData.Load();

        FirePlaying = false;
        RainPlaying = false;

        Instance = this;
    }

    private void CreateSaveData()
    {
        SaveData = SaveData.Create("TaskerSessions");
    }

    public void VoidSaveData()
    {
        SaveData.Delete();
        CreateSaveData();
        SaveData.Save();
    }
}
