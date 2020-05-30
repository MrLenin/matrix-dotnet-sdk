using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Matrix.Api.ClientServer;
using Matrix.Api.ClientServer.Enumerations;

namespace Matrix.Api
{
    public interface IRequest
    {
        RequestKind RequestKind { get; }
        Uri Path { get; }
        IEnumerable<byte> Content { get; }

        [DataMember(Name = @"auth")]
        AuthenticationRequest? Authentication { get; }
    }
}
