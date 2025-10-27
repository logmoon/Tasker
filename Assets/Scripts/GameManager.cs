using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public SaveData SaveData { get; private set; }
    public bool FirePlaying { get; set; }
    public bool RainPlaying { get; set; }

    public void Save()
    {
        SaveData.Save();
    }
    public void Load()
    {
        SaveData.Load();
    }

    private void Awake()
    {
        // Load save data
        CreateSaveData();
        Load();

        // Set FPS cap based on saved setting
        SetFpsCap(SaveData.FPSCapSetting);

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
        Save();
    }

    public void SetFpsCap(FPSCapSetting setting, bool save = false)
    {
        switch (setting)
        {
            case FPSCapSetting.SIXTY:
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 60;
                SaveData.FPSCapSetting = FPSCapSetting.SIXTY;
                break;
            case FPSCapSetting.THIRTY:
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 30;
                SaveData.FPSCapSetting = FPSCapSetting.THIRTY;
                break;
            case FPSCapSetting.MONITOR:
                QualitySettings.vSyncCount = 1;
                Application.targetFrameRate = 60;
                SaveData.FPSCapSetting = FPSCapSetting.MONITOR;
                break;
            default:
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 60;
                SaveData.FPSCapSetting = FPSCapSetting.SIXTY;
                break;
        }

        if (save)
        {
            SaveData.Save();
        }
    }

    public int GetFpsCapForMonitor()
    {
        var ratio = Screen.currentResolution.refreshRateRatio;
        return (int)((float)ratio.numerator / ratio.denominator);
    }
}
