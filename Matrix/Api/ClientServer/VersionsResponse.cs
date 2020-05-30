using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;

using Matrix.Api.Versions;

namespace Matrix.Api.ClientServer
{
    public class VersionsResponse : IResponse
    {
        [DataMember(Name = @"versions")]
        public IEnumerable<ClientServerApiVersion> ApiVersions { get; set; }
        [DataMember(Name = @"unstable_features")]
        public IDictionary<string, bool> UnstableFeatures { get; set; }

        public VersionsResponse()
        {
            ErrorCode = ErrorCode.None;
            HttpStatusCode = HttpStatusCode.OK;
            ApiVersions = new List<ClientServerApiVersion>();
            UnstableFeatures = new Dictionary<string, bool>();
        }

        public ErrorCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public uint RetryAfterMilliseconds { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }

}