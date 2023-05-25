using System;

namespace DolDoc.Centaur.Internals
{
    public class CDate
    {
        public uint time;
        public int date;
    }

    public class CDateStruct
    {
        public byte sec10000, sec100, sec, min, hour, day_of_week, day_of_mon, mon;
        public int year;
    }

    public class Chrono
    {
        public const int CDATE_BASE_DAY_OF_WEEK = 0;
        public const int CDATE_YEAR_DAYS_INT = 36524225;

        private static ushort[] mon_start_days1 =
        {
            0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334
        };
 
        private static ushort[] mon_start_days2 =
        {
            0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335
        };

        public static CDate Now()
        {
            var now = NowDateTimeStruct();
            return Struct2Date(now);
        }

        public static long DayOfWeek(long i)
        {
            i += CDATE_BASE_DAY_OF_WEEK;
            if (i >= 0)
                return i % 7;
            return 6 - (6 - i) % 7;
        }

        public static CDateStruct Date2Struct(CDate dt)
        {
            CDateStruct ds = new CDateStruct();
            long i, k, date = dt.date;
            ds.day_of_week = (byte) DayOfWeek(date);
            ds.year = (int) ((date + 1) * 100000 / CDATE_YEAR_DAYS_INT);
            i = YearStartDate(ds.year);
            while (i > date)
            {
                ds.year--;
                i = YearStartDate(ds.year);
            }

            date -= i;
            if (YearStartDate(ds.year + 1) - i == 365)
            {
                k = 0;
                while (date >= mon_start_days1[k + 1] && k < 11)
                    k++;
                date -= mon_start_days1[k];
            }
            else
            {
                k = 0;
                while (date >= mon_start_days2[k + 1] && k < 11)
                    k++;
                date -= mon_start_days2[k];
            }

            ds.mon = (byte) (k + 1);
            ds.day_of_mon = (byte) (date + 1);
            k = (625 * 15 * 15 * 3 * dt.time) >> 21 + 1;
            ds.sec10000 = (byte) (k %= 100);
            ds.sec100 = (byte) (k %= 100);
            ds.sec = (byte) (k %= 100);
            ds.min = (byte) (k %= 100);
            ds.hour = (byte) k;
            return ds;
        }

        public static CDate Struct2Date(CDateStruct ds)
        {
            var cdt = new CDate();
            var i1 = YearStartDate(ds.year);
            var i2 = YearStartDate(ds.year + 1);
            if (i2 - i1 == 365)
                i1 += mon_start_days1[ds.mon - 1];
            else
                i1 += mon_start_days2[ds.mon - 1];
            cdt.date = (int) i1 + ds.day_of_mon - 1;
            cdt.time = (uint) (((ds.sec10000 + 100) * ((ds.sec100 + 100) * ((ds.sec + 60) * ((ds.min + 60) * ds.hour)))) << 21 / (15 * 15 * 3 * 625));
            return cdt;
        }

        public static long YearStartDate(long year)
        {
            long y1 = year - 1, yd4000 = y1 / 4000, yd400 = y1 / 400, yd100 = y1 / 100, yd4 = y1 / 4;
            return year * 365 + yd4 - yd100 + yd400 - yd4000;
        }

        public static CDateStruct NowDateTimeStruct()
        {
            var now = DateTime.Now;
            return new CDateStruct
            {
                hour = (byte) now.Hour,
                min = (byte) now.Minute,
                day_of_mon = (byte) now.Day,
                day_of_week = (byte) now.DayOfWeek,
                mon = (byte) now.Month,
                sec = (byte) now.Second,
                year = now.Year
            };
        }

        public static string MPrintDate(CDate cdt)
        {
            var ds = Date2Struct(cdt);
            return $"{ds.day_of_mon,2:00}/{ds.mon,2:00}/{ds.year % 100,2:00}";
        }

        public static string MPrintTime(CDate cdt)
        {
            var ds = Date2Struct(cdt);
            return $"{ds.hour,2:00}:{ds.min,2:00}:{ds.sec,2:00}";
        }
    }
}