using System.Net;

using Matrix.Api;
using Matrix.Api.ClientServer.Enumerations;

namespace Matrix.Backends
{
    public class MatrixRequestError
    {
        public string MatrixError { get; }
        public ErrorCode MatrixErrorCode { get; }
        public HttpStatusCode Status { get; }
        public int RetryAfter { get; }
        public bool IsOk => MatrixErrorCode == ErrorCode.None && Status == HttpStatusCode.OK;

        public MatrixRequestError(string merror, ErrorCode code, HttpStatusCode status, int retryAfter = -1)
        {
            MatrixError = merror;
            MatrixErrorCode = code;
            Status = status;
            RetryAfter = retryAfter;
        }

        public string GetErrorString()
        {
            if (Status != HttpStatusCode.OK) return "Got a Http Error :" + Status + " during request.";

            return "Got a Matrix Error: " + MatrixErrorCode + " '" + MatrixError + "'";
        }

        public override string ToString()
        {
            return GetErrorString();
        }

        public static readonly MatrixRequestError NoError =
            new MatrixRequestError(
                "",
                ErrorCode.None,
                HttpStatusCode.OK
            );
    }
}