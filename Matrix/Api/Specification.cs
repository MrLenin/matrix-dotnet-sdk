
using System.IO;

namespace Matrix.Api
{
    public enum Specification
    {
        ClientServer,
        ServerServer,
        ApplicationService,
        IdentityService,
        PushGateway,
        Rooms
    }

    public static class SpecificationExtensions
    {
        public static string ToJsonString(this Specification clientServerApiVersion)
        {
            return clientServerApiVersion switch
            {
                Specification.ClientServer => @"client_server",
                Specification.ServerServer => @"server_server",
                Specification.ApplicationService => @"application_service",
                Specification.IdentityService => @"identity_service",
                Specification.PushGateway => @"push_gateway",
                Specification.Rooms => @"rooms",
                _ => throw  new InvalidDataException()
            };
        }

        public static Specification FromJsonString(this string clientServerApiVersion)
        {
            return clientServerApiVersion switch
            {
                @"client_server" => Specification.ClientServer,
                @"server_server" => Specification.ServerServer,
                @"application_service" => Specification.ApplicationService,
                @"identity_service" => Specification.IdentityService,
                @"push_gateway" => Specification.PushGateway,
                @"rooms" => Specification.Rooms,
                _ => throw new InvalidDataException()
            };
        }
    }
}