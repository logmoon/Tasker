using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlertMenuResetController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    private Action _yesButtonPressedAction;
    private Action _noButtonPressedAction;

    public void SetupDialog(string message, Action yesButtonPressed, Action noButtonPressed)
    {
        messageText.text = message;
        _yesButtonPressedAction = yesButtonPressed;
        _noButtonPressedAction = noButtonPressed;
    }

    private void OnDisable()
    {
        ResetDialog();
    }

    public void YesButton()
    {
        _yesButtonPressedAction?.Invoke();
    }
    public void NoButton()
    {
        _noButtonPressedAction?.Invoke();
    }

    private void ResetDialog()
    {
        _yesButtonPressedAction = null;
        _noButtonPressedAction = null;
    }
}
