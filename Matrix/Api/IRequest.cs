using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Matrix.Api.ClientServer;
using Matrix.Api.ClientServer.Enumerations;

using Newtonsoft.Json;

namespace Matrix.Api
{
    public interface IRequest
    {
        RequestKind RequestKind { get; }
        Uri Path { get; }
        IEnumerable<byte> Content { get; }

        [JsonProperty(@"auth")]
        AuthRequest? Auth { get; }
    }
}
