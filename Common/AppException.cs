using System;
using System.Diagnostics;

namespace REMM.Common
{
    public class AppException : Exception
    {
        public AppException(string message) : base(message) => Debug.WriteLine(message);
        public AppException(string message, Exception innerException) : base(message, innerException) => Debug.WriteLine(message);
    }
}
