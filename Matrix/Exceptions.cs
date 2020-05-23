using System;
using Matrix.Backends;
using Newtonsoft.Json.Linq;

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

    public class MatrixServerError : MatrixException
    {
        public MatrixErrorCode ErrorCode { get; }
        public string ErrorCodeStr { get; }
        public JObject ErrorObject { get; }

        public MatrixServerError(string errorcode, string message, JObject errorObject) : base(message)
        {
            if (!Enum.TryParse(errorcode, out MatrixErrorCode matrixErrorCode))
            {
                ErrorCode = MatrixErrorCode.UnknownErrorCode;
                ErrorObject = errorObject;
            }

            ErrorCode = matrixErrorCode;
            ErrorCodeStr = errorcode;
        }

        public MatrixServerError()
        {
        }

        public MatrixServerError(string message) : base(message)
        {
        }

        public MatrixServerError(string message, Exception innerException) : base(message, innerException)
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