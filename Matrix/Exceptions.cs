using System;

using Matrix.Api;
using Matrix.Backends;

namespace Matrix
{
    public class MatrixException : Exception
    {
        public MatrixException(string message) : base(message)
        {
        }

        public MatrixException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public MatrixException()
        {
        }
    }

    public class MatrixBadFormatException : MatrixException
    {
        public MatrixBadFormatException(string value, string type, string reason) 
            : base($"Value \"{value}\" is not valid for type {type}, Reason: {reason}")
        {
        }

        public MatrixBadFormatException()
        {
        }

        public MatrixBadFormatException(string message) : base(message)
        {
        }

        public MatrixBadFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}