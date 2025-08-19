namespace Antura.Discover
{
    public static class DatetimeUtilities
    {
        /// <summary>
        /// Returns the current date and time in a format suitable for saving to a file.
        /// </summary>
        public static string GetNowUtcString()
        {
            return System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        }
    }
}
