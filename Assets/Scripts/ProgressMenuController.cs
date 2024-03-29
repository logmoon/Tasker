using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressMenuController : MonoBehaviour
{
    [SerializeField] private GameObject noSessionsCompleteText;
    [SerializeField] private Transform completedSessionsListParent;
    [SerializeField] private GameObject completedSessionPrefab;

    private void OnEnable()
    {
        var sessions = GameManager.Instance.SaveData.Sessions;

        if (sessions.Count == 0)
        {
            noSessionsCompleteText.SetActive(true);
            return;
        }
        else
        {
            noSessionsCompleteText.SetActive(false);
        }

        foreach (var session in sessions)
        {
            var sessionGameobject = Instantiate(completedSessionPrefab, completedSessionsListParent);

            // Convert the float timer value to a TimeSpan
            TimeSpan timeSpan = TimeSpan.FromSeconds(session.Duration * 60f);
            // Format the TimeSpan
            string formattedTimer;
            if (timeSpan.Hours >= 1)
            {
                // Format the TimeSpan as "hh:mm:ss"
                formattedTimer = string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            }
            else
            {
                // Format the TimeSpan as "mm:ss" when it's less than an hour
                formattedTimer = string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
            }

            sessionGameobject.GetComponentInChildren<TextMeshProUGUI>().text =
                $"Name: {session.Name} | Duration: {formattedTimer}";
        }
    }

    private void OnDisable()
    {
        // Destroy all completed sessions
        for (var i = completedSessionsListParent.childCount - 1; i >= 0; i--)
        {
            Destroy(completedSessionsListParent.GetChild(i).gameObject);
        }
    }

    public void BackButton()
    {
        MenuManager.Instance.TurnMenuOff(MenuIndexes.Progress, MenuIndexes.Setup, true);
    }
}
