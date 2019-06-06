using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UtilPack4Unity
{
    public class DateTimeInfo
    {
        public int Year;
        public int Month;
        public int Day;
        public int Hour;
        public int Minute;
        public int Second;
        public int Millisecond;

        public DateTimeInfo() { }
        public DateTimeInfo(DateTime dateTime)
        {
            this.Year = dateTime.Year;
            this.Month = dateTime.Month;
            this.Day = dateTime.Day;
            this.Hour = dateTime.Hour;
            this.Minute = dateTime.Minute;
            this.Second = dateTime.Second;
            this.Millisecond = dateTime.Millisecond;
        }

        public DateTime ToDateTime()
        {
            return new DateTime(Year, Month, Day, Hour, Minute, Second, Millisecond);
        }
    }
}
