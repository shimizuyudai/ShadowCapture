using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UtilPack4Unity
{
    public static class DateTimeUitls
    {
        public static long DateTime2UnixTime(DateTime dateTime)
        {
            var dateTimeOffset = new DateTimeOffset(dateTime.Ticks, new TimeSpan(+09, 00, 00));
            return dateTimeOffset.ToUnixTimeSeconds();
        }

        public static DateTime UnixTime2DateTime(long unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).LocalDateTime;
        }

    }
}
