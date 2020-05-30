﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

using Matrix.Api.ClientServer.Enumerations;
using Matrix.Api.Versions;

namespace Matrix.Api.ClientServer
{
    public class VersionsResponse : IResponse
    {
        [DataMember(Name = @"versions")]
        public IEnumerable<ClientServerVersion> ApiVersions { get; set; }
        [DataMember(Name = @"unstable_features")]
        public IDictionary<string, bool> UnstableFeatures { get; set; }

        public VersionsResponse()
        {
            ErrorCode = ErrorCode.None;
            HttpStatusCode = HttpStatusCode.OK;
            ApiVersions = new List<ClientServerVersion>();
            UnstableFeatures = new Dictionary<string, bool>();
        }

        public ErrorCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public uint RetryAfterMilliseconds { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }

}