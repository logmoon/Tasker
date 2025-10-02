using System;

// For now a session data implies that it is done
[System.Serializable]
public class SessionData
{
    public string Name;
    public int Day;
    public int Month;
    public int Year;
    public int Hour;
    public int Minute;
    public int Second;
    public float Duration;

    public void SetDate(int y, int mon, int d, int h, int min, int s)
    {
        this.Year = y;
        this.Month = mon;
        this.Day = d;
        this.Hour = h;
        this.Minute = min;
        this.Second = s;
    }

    public void SetDate(DateTime date)
    {
        SetDate(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
    }

    public DateTime GetDate()
    {
        return new DateTime(Year, Month, Day, Hour, Minute, Second);
    }
}
