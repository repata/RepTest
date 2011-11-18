using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace unCCed
{
    /// <summary>
    /// Summary description for unCCed
    /// </summary>
    public class Utilities
    {
        public Utilities()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static bool checkEmail(string email) {
           return System.Text.RegularExpressions.Regex.IsMatch(email, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");


        }

        public static string getTime(int secs)
        { 
            TimeSpan ts = DateTime.Now - new DateTime (2009,12,1,0,0,0);
            int csecs = (int)Math.Round(ts.TotalSeconds, 0);
            int tsecs = csecs - secs;
            double tdiff = tsecs;
            double weekn = (60 * 60 * 24 * 7);
            double dayn = 60 * 60 * 24;
            double hourn = 60 * 60;
            double minn = 60;
            double weeks = Math.Floor(tdiff / weekn);
            tdiff -= (weeks * weekn);

            double days = Math.Floor(tdiff / dayn);
            tdiff -= (days * dayn);

            double hours = Math.Floor(tdiff / hourn);
            tdiff -= (hours * hourn);

            double mins = Math.Floor(tdiff / minn);
            tdiff -= (mins * minn);

            double sec = tdiff;

            if (weeks > 0)
                return weeks + " " + ((weeks == 1) ? "week" : "weeks") + " ago";
            else if (days > 0)
                return days + " " + ((days == 1) ? "day" : "days") + " ago";
            else if (hours > 0)
                return hours + " " + ((hours == 1) ? "hr" : "hrs") + " ago";
            else if (mins > 0)
                return mins + " " + ((mins == 1) ? "min" : "mins") + " ago";
            else
                return sec + " " + ((sec == 1) ? "sec" : "secs") + " ago";

            //if (tsecs < 60)
            //    return tsecs.ToString() + " secs ago";
            //else if (tsecs < (60 * 60))
            //    return Math.Round((double)(tsecs / 60),0).ToString() + " mins ago";
            //else if (tsecs < (60 * 60 * 24))
            //    return ((tsecs / 60) % 24).ToString() + " hours ago";
            //else
            //    return tsecs.ToString();
        }
        public static string getTime(DateTime date)
        {
            string r = String.Empty;

            TimeSpan ts = DateTime.Now - date;

            if (ts.TotalSeconds < 60)
                return Math.Round(ts.TotalSeconds,0).ToString() + " secs ago";
            else if (ts.TotalMinutes < 60)
                return Math.Round(ts.TotalMinutes,0).ToString() + " mins ago";
            else if (ts.TotalHours < 24)
                return Math.Round(ts.TotalHours,0).ToString() + " hours ago";
            else
                return Math.Round(ts.TotalDays, 0).ToString() + " days ago";

            
        }

        public static string GenerateRandomString(int numChars)
        {
            string[] strCharacters = { "A","B","C","D","E","F","G",
"H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y",
"Z","1","2","3","4","5","6","7","8","9","0","a","b","c","d","e","f","g","h",
"i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"
,"0","1","2","3","4","5","6","7","8","9"};
            Random r = new Random();
            int p = 0;
            String ret = String.Empty;

            for (int x=0;x<=numChars;x++)
            {
                p = r.Next(0,70);
                    ret += strCharacters[p];
            }

            return ret;
        }
    }
}