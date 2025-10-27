using System;
using System.Collections.Generic;
using UnityEngine;

public enum FPSCapSetting
{
    SIXTY,
    THIRTY,
    MONITOR,
}

public class SaveData : SmartSaves.Data<SaveData>
{
    public const int CURRENT_VERSION = 2;

    // Settings
    public FPSCapSetting FPSCapSetting = FPSCapSetting.SIXTY;

    // Sessions
    public List<SessionData> Sessions = new List<SessionData>();
    public int Version = 0;

    protected override void OnAfterLoad()
    {
        if (Version < CURRENT_VERSION)
        {
            Debug.LogWarning("SaveData version is outdated. Attempting migration.");
            HandleMigrations();
        }
    }

    private void HandleMigrations()
    {
        Debug.Log($"Migrating from version: ({Version}) to version: ({CURRENT_VERSION})");

        // =============================================================================
        // VERSION 1 MIGRATION:
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
        // =============================================================================

        // =============================================================================
        // VERSION 2 MIGRATION:
        FPSCapSetting = FPSCapSetting.SIXTY;
        // =============================================================================

        // Update the version
        Version = CURRENT_VERSION;

        this.Save();

        Debug.Log("Migration complete.");
    }
}
