using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Matrix.Backends
{
    public interface IMatrixApiBackend
    {
        MatrixRequestError HandleGet(Uri apiPath, bool authenticate, out JToken result);
        Task<MatrixApiResult> HandleGetAsync(Uri apiPath, bool authenticate);
        MatrixRequestError HandleDelete(Uri apiPath, bool authenticate, out JToken result);
        MatrixRequestError HandlePost(Uri apiPath, bool authenticate, JToken request, out JToken result);
        MatrixRequestError HandlePut(Uri apiPath, bool authenticate, JToken request, out JToken result);
        Task<MatrixApiResult> HandlePutAsync(Uri apiPath, bool authenticate, JToken request);

        MatrixRequestError HandlePost(Uri apiPath, bool authenticate, JToken request, Dictionary<string, string> headers,
            out JToken result);

        MatrixRequestError HandlePost(Uri apiPath, bool authenticate, byte[] request, Dictionary<string, string> headers,
            out JToken result);

        void SetAccessToken(string accessToken);
    }

    public struct MatrixApiResult : IEquatable<MatrixApiResult>
    {
        public JToken Result { get; set; }
        public MatrixRequestError Error { get; set; }

        public bool Equals(MatrixApiResult other)
        {
            return (Result == other.Result) && (Error == other.Error);
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                var other = (MatrixApiResult) obj;
                return Equals(other);
            }
        }

        public override int GetHashCode()
        {
            return Result.GetHashCode() ^ Error.GetHashCode();
        }

        public static bool operator ==(MatrixApiResult left, MatrixApiResult right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MatrixApiResult left, MatrixApiResult right)
        {
            return !(left == right);
        }
    }
}