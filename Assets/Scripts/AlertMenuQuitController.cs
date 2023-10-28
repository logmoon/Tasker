using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertMenuQuitController : MonoBehaviour
{
    public void YesButton()
    {
        Application.Quit();
    }
    public void NoButton()
    {
        MenuManager.Instance.TurnMenuOff(MenuIndexes.AlertQuit, MenuIndexes.Main, true);
        Time.timeScale = 1.0f;
    }
}
