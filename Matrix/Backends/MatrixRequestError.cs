using System.Net;

namespace Matrix.Backends
{
    public class MatrixRequestError
    {
        public string MatrixError { get; }
        public MatrixErrorCode MatrixErrorCode { get; }
        public HttpStatusCode Status { get; }
        public int RetryAfter { get; }
        public bool IsOk => MatrixErrorCode == MatrixErrorCode.None && Status == HttpStatusCode.OK;

        public MatrixRequestError(string merror, MatrixErrorCode code, HttpStatusCode status, int retryAfter = -1)
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
                MatrixErrorCode.None,
                HttpStatusCode.OK
            );
    }
}