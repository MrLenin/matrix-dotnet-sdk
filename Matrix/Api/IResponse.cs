using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

using Matrix.Api.ClientServer.Enumerations;

using Newtonsoft.Json;

namespace Matrix.Api
{
    public interface IResponse
    {
        [JsonProperty(@"errcode")]
        ErrorCode? ErrorCode { get; set; }
        [JsonProperty(@"error")]
        string? ErrorMessage { get; set; }
        [JsonProperty(@"retry_after_ms")]
        long? RetryAfterMilliseconds { get; set; }
        HttpStatusCode HttpStatusCode { get; set; }
    }
}