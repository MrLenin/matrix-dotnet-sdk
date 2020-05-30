using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Matrix.Api.ClientServer;

namespace Matrix.Api
{
    public enum RequestKind
    {
        Post,
        Put,
        Delete
    }

    public interface IRequest
    {
        RequestKind RequestKind { get; }
        Uri Path { get; }
        IEnumerable<byte> Content { get; }

        [DataMember(Name = @"auth")]
        AuthenticationRequest? Authentication { get; }
    }
}
