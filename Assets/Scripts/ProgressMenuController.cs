using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressMenuController : MonoBehaviour
{
    [SerializeField] private GameObject noSessionsCompleteText;
    [SerializeField] private Transform completedSessionsListParent;
    [SerializeField] private Image scrollImage; 
    [SerializeField] private CompletedSessionOverview completedSessionPrefab;
    [SerializeField] private GameObject completedSessionDatePrefab;

    private void SpawnCompletedSession(SessionData session)
    {
        CompletedSessionOverview completedSession = Instantiate(completedSessionPrefab, completedSessionsListParent);

        completedSession.titleText.text = session.Name;

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

        completedSession.durationText.text = formattedTimer;

        // Format date as dd/mm/yy - hh:mm AM/PM
        string formattedDate = session.GetDate().ToString("dd/MM/yy - hh:mm tt");
        completedSession.dateText.text = formattedDate;
    }

    private void OnEnable()
    {
        var sessions = GameManager.Instance.SaveData.Sessions;

        if (sessions.Count == 0)
        {
            noSessionsCompleteText.SetActive(true);
            Color org = scrollImage.color;
            scrollImage.color = new Color(org.r, org.g, org.b, 0.0f);
            return;
        }
        else
        {
            noSessionsCompleteText.SetActive(false);
            Color org = scrollImage.color;
            scrollImage.color = new Color(org.r, org.g, org.b, 1.0f/255.0f);
        }

        // Sort by date (latest date first)
        var sortedSessions = new List<SessionData>(sessions);
        sortedSessions.Sort((a, b) => DateTime.Compare(b.GetDate(), a.GetDate()));

        // Track the current date to detect changes
        DateTime? currentDate = null;

        foreach (var session in sortedSessions)
        {
            // Check if the date has changed (ignoring time)
            DateTime sessionDate = session.GetDate();

            if (currentDate == null || sessionDate.Date != currentDate.Value.Date)
            {
                // Date has changed, add a header
                var header = Instantiate(completedSessionDatePrefab, completedSessionsListParent);
                header.GetComponentInChildren<TextMeshProUGUI>().text = session.GetDate().ToString("dd/MM/yyyy");
                
                // Update current date
                currentDate = sessionDate;
            }
            
            // Spawn the session
            SpawnCompletedSession(session);
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
