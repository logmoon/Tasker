using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertMenuResetController : MonoBehaviour
{
    public void YesButton()
    {
        MenuManager.Instance.TurnMenuOff(MenuIndexes.AlertReset, MenuIndexes.Setup, true);
        Time.timeScale = 1.0f;
    }
    public void NoButton()
    {
        MenuManager.Instance.TurnMenuOff(MenuIndexes.AlertReset, MenuIndexes.Main, true);
        Time.timeScale = 1.0f;
    }
}
