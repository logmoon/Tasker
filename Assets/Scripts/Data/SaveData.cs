using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : SmartSaves.Data<SaveData>
{
    public const int CURRENT_VERSION = 1;

    // Sessions
    public List<SessionData> Sessions = new List<SessionData>();
    public int Version = 0;

    protected override void OnAfterLoad()
    {
        if (Version < CURRENT_VERSION)
        {
            Debug.LogWarning("SaveData version is outdated. Attempting migration.");
            MigrateSaveData();
        }
    }

    private void MigrateSaveData()
    {
        foreach (var session in Sessions)
        {
            // =============================================================================
            // VERSION 1 MIGRATION:
            if (session.Year == 0 && session.Month == 0 && session.Day == 0)
            {
                session.SetDate(DateTime.Now);
                Debug.Log($"Migrated session '{session.Name}' to current date.");
            }
            // =============================================================================
        }

        // Update the version
        Version = CURRENT_VERSION;
        Debug.Log("SaveData migration complete.");
    }
}
