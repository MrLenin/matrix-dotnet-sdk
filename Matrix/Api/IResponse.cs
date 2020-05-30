using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

using Matrix.Backends;

namespace Matrix.Api
{
    public interface IResponse
    {
        [DataMember(Name = @"errcode")]
        ErrorCode ErrorCode { get; set; }
        [DataMember(Name = @"error")]
        string ErrorMessage { get; set; }
        [DataMember(Name = @"retry_after_ms")]
        uint RetryAfterMilliseconds { get; set; }
        HttpStatusCode HttpStatusCode { get; set; }
    }
}