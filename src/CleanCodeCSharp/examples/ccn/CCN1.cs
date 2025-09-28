using System;

namespace CleanCodeCSharp.Examples.Ccn
{
    public class CCN1
    {
        public int GetPort()
        {
            string? portAsString = Environment.GetEnvironmentVariable("port");
            if (portAsString == null)
            {
                return 8080;
            }
            try
            {
                return int.Parse(portAsString);
            }
            catch (FormatException)
            {
                return 8080;
            }
        }
    }
}