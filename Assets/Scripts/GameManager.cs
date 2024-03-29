using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public SaveData SaveData { get; private set; }

    private void Awake()
    {
        Instance = this;

        SaveData = SaveData.Create("SaveData");
        SaveData.Load();
    }
}
