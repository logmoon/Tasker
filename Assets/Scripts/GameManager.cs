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
        SaveData = SaveData.Create("SaveData");
        SaveData.Load();
        SaveData.Save();

        FirePlaying = false;
        RainPlaying = false;

        Instance = this;
    }
}
