using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public void ResumeButton()
    {
        MenuManager.Instance.TurnMenuOff(MenuIndexes.Pause, MenuIndexes.Main, true);
        Time.timeScale = 1.0f;
    }
}
