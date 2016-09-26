using System;

namespace App1
{
    internal class ApplicationException : Exception
    {
        public ApplicationException()
        {
        }

        public ApplicationException(string message) : base(message)
        {
            message="Somthing wrong";
        }

        public ApplicationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}