using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : SmartSaves.Data<SaveData>
{
    // Sessions
    public List<SessionData> Sessions = new List<SessionData>();
}
