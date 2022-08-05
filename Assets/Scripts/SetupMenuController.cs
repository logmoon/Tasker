using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class SetupMenuController : MonoBehaviour
{
    [SerializeField] private MainMenuController mainMenu;
    [SerializeField] private TextMeshProUGUI inputText;
    [SerializeField] private TextMeshProUGUI taskNameText;

    private void OnEnable()
    {
        inputText.text = "";
        taskNameText.text = "";
    }

    public void StartButton()
    {
        float time = 0.0f;

        string s = inputText.text.Substring(0, inputText.text.Length - 1); 
        if (float.TryParse(s, out time) && taskNameText.text != "")
        {
            mainMenu.StartTimer(time, taskNameText.text);
            MenuManager.Instance.TurnMenuOff(MenuIndexes.Setup, MenuIndexes.Main, true);
        }
    }
}
