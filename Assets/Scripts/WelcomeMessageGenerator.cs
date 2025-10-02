using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WelcomeMessageGenerator
{
    private static readonly string[] greetingMessages = {
        "Oh hey",
        "Sup :)",
        "How you doin'?",
        "Welcome back",
        "Another day, another victory for the og",
        "You again huh",
        "Back for more?",
        "Time to lock in"
    };

    public static string GetMessage()
    {
        SaveData saveData = GameManager.Instance.SaveData;

        if (UnityEngine.Random.value < 0.25f && CanShowStats(saveData))
        {
            return GetStatsMessage(saveData);
        }

        return GetGreetingMessage();
    }

    private static bool CanShowStats(SaveData saveData)
    {
        if (saveData.Sessions == null || saveData.Sessions.Count < 3)
            return false;

        DateTime now = DateTime.Now;
        bool hasRecentActivity = saveData.Sessions.Any(s =>
            (now - s.GetDate()).TotalDays <= 30
        );

        return hasRecentActivity;
    }

    private static string GetStatsMessage(SaveData saveData)
    {
        List<string> possibleStats = new List<string>();

        int totalSessions = saveData.Sessions.Count;
        float totalHours = saveData.Sessions.Sum(s => s.Duration) / 3600f;

        // Total sessions
        if (totalSessions >= 5)
        {
            possibleStats.Add($"{totalSessions} sessions. Not bad");

            if (totalSessions % 10 == 0)
                possibleStats.Add($"{totalSessions} sessions done. Keep it up!");

            if (totalSessions >= 25)
                possibleStats.Add($"{totalSessions} sessions, damn");
        }

        // Total hours
        if (totalHours >= 5f)
        {
            possibleStats.Add($"{totalHours:F1} hours so far");

            if (totalHours >= 10f)
                possibleStats.Add($"{totalHours:F0} hours in the books");

            if (totalHours >= 25f)
                possibleStats.Add($"{totalHours:F0} hours. Damn");

            if (totalHours >= 50f)
                possibleStats.Add($"{totalHours:F0} hours. Ok wow");
        }

        // Streak
        int currentStreak = CalculateStreak(saveData);
        if (currentStreak >= 2)
        {
            possibleStats.Add($"{currentStreak} days in a row somehow");

            if (currentStreak >= 5)
                possibleStats.Add($"{currentStreak} day streak going");

            if (currentStreak >= 7)
                possibleStats.Add($"{currentStreak} days straight. Respect");

            if (currentStreak >= 14)
                possibleStats.Add($"{currentStreak} day streak, you good?");
        }

        // This week's stats
        DateTime weekAgo = DateTime.Now.AddDays(-7);
        var thisWeekSessions = saveData.Sessions.Where(s => s.GetDate() >= weekAgo).ToList();

        if (thisWeekSessions.Count >= 3)
        {
            float weekHours = thisWeekSessions.Sum(s => s.Duration) / 3600f;
            possibleStats.Add($"{thisWeekSessions.Count} sessions this week");

            if (weekHours >= 5f)
                possibleStats.Add($"{weekHours:F1} hours this week nice");

            if (thisWeekSessions.Count >= 5)
                possibleStats.Add($"{thisWeekSessions.Count} this week. On a roll");
        }

        if (possibleStats.Count > 0)
        {
            return possibleStats[UnityEngine.Random.Range(0, possibleStats.Count)];
        }

        return GetGreetingMessage();
    }

    private static int CalculateStreak(SaveData saveData)
    {
        if (saveData.Sessions.Count == 0) return 0;

        var sessionDays = saveData.Sessions
            .Select(s => s.GetDate().Date)
            .Distinct()
            .OrderByDescending(d => d)
            .ToList();

        if (sessionDays.Count == 0) return 0;

        DateTime today = DateTime.Now.Date;
        DateTime mostRecent = sessionDays[0];

        if ((today - mostRecent).TotalDays > 1)
            return 0;

        int streak = 1;
        DateTime expectedDate = mostRecent.AddDays(-1);

        for (int i = 1; i < sessionDays.Count; i++)
        {
            if (sessionDays[i] == expectedDate)
            {
                streak++;
                expectedDate = expectedDate.AddDays(-1);
            }
            else
            {
                break;
            }
        }

        return streak;
    }

    private static string GetGreetingMessage()
    {
        if (UnityEngine.Random.value < 0.5f)
        {
            return GetTimeBasedGreeting();
        }
        return greetingMessages[UnityEngine.Random.Range(0, greetingMessages.Length)];
    }

    private static string GetTimeBasedGreeting()
    {
        int hour = DateTime.Now.Hour;

        // Early Morning (5 AM - 8 AM)
        if (hour >= 5 && hour < 8)
        {
            string[] early = {
                "Life waits for no one, good to see you here",
                "Early bird huh?",
                "Up with the sun?",
                "Starting early today",
                "Crack of dawn vibes",
                "Morning already, damn",
                "The world is quiet now",
                "First one up?",
                "Sunrise session incoming"
            };
            return early[UnityEngine.Random.Range(0, early.Length)];
        }
        // Morning (8 AM - 12 PM)
        else if (hour >= 8 && hour < 12)
        {
            string[] morning = {
                "Morning :)",
                "Good morning!",
                "How'd you sleep?",
                "Fresh start?",
                "Ready to begin?",
                "What's on the agenda?",
                "New day, new session",
                "Starting strong?",
                "Morning clarity hits different"
            };
            return morning[UnityEngine.Random.Range(0, morning.Length)];
        }
        // Afternoon (12 PM - 5 PM)
        else if (hour >= 12 && hour < 17)
        {
            string[] afternoon = {
                "Good afternoon!",
                "How's your day going?",
                "Midday check-in?",
                "Post-lunch clarity?",
                "Halfway there",
                "Keeping the momentum?",
                "Second wind incoming",
                "What are we working on?",
                "Time to refocus?"
            };
            return afternoon[UnityEngine.Random.Range(0, afternoon.Length)];
        }
        // Evening (5 PM - 9 PM)
        else if (hour >= 17 && hour < 21)
        {
            string[] evening = {
                "How was your day?",
                "Getting late, no?",
                "One more session?",
                "Wrapping things up?",
                "End of day push?",
                "Sunset already, huh",
                "Almost done for today?",
                "Final stretch?",
                "Finishing strong?"
            };
            return evening[UnityEngine.Random.Range(0, evening.Length)];
        }
        // Late Night (9 PM - 5 AM)
        else
        {
            string[] night = {
                "Night owl mode activated",
                "Kinda late, no?",
                "Burning midnight oil?",
                "Can't sleep?",
                "The night is yours",
                "Working late huh",
                "When everyone else sleeps",
                "Still at it?",
                "The grind never stops, I guess"
            };
            return night[UnityEngine.Random.Range(0, night.Length)];
        }
    }
}