using System;

namespace SNMPManager.Core.Sonars.Exceptions
{
    public class SonarMissingException : Exception
    {
        public SonarMissingException()
        {
            
        }

        public SonarMissingException(string message)
            :base(message)
        {
            
        }

        public SonarMissingException(string message, Exception innerException)
            :base(message, innerException)
        {
            
        }
    }
}