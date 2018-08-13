using System;

namespace Simple_Twitter.Utilities
{
    public static class ElaspedTimeString
    {
        public static string GetElaspedTimeString(DateTime time)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            TimeSpan elapsed = DateTime.Now - time;
            string elapsedTime;
            string plural;

            if (elapsed.Days >= 365)                       
            {
                plural = (elapsed.Days / 365) >= 2 ? "s" : "";
                elapsedTime = string.Format("{0} year{1} ago", (int)(elapsed.Days / 365), plural);
            }
            else if (elapsed.Days >= 1)
            {
                plural = (elapsed.Days > 1) ? "s" : "";
                elapsedTime = string.Format("{0} day{1} ago", elapsed.Days, plural);
            }
            else if (elapsed.Hours >= 1)
            {
                plural = elapsed.Hours > 1 ? "s" : "";
                elapsedTime = string.Format("{0} hour{1} ago", elapsed.Hours, plural);
            }
            else if (elapsed.Minutes >= 1)
            {
                plural = elapsed.Minutes > 1 ? "s" : "";
                elapsedTime = string.Format("{0} minute{1} ago", elapsed.Minutes, plural);
            }
            else
            {
                plural = elapsed.Seconds > 1 ? "s" : "";
                elapsedTime = string.Format("{0} second{1} ago", elapsed.Seconds, plural);
            }
            return elapsedTime;
        }
    }
}