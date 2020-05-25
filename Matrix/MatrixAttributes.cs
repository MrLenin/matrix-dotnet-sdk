using System;
using System.Collections.Generic;

using Matrix.Api.Versions;

using Newtonsoft.Json.Converters;

using YamlDotNet.Serialization;

using static Matrix.Api.Versions.ApiVersionContext;

namespace Matrix
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MatrixSpecAttribute : Attribute
    {
        private const string MatrixSpecUrl = "http://matrix.org/docs/spec/";

        private readonly ApiVersionContext _apiVersionContext;

        internal ApiVersionContext ApiVersionContext => _apiVersionContext;

        public string Path { get; }

        public MatrixSpecAttribute(ClientServerApiVersion supportedVer, string path,
            ClientServerApiVersion lastVersion = ClientServerApiVersion.Unknown)
        {
            Path = path;
            _apiVersionContext = new ClientServer(supportedVer, lastVersion);
        }

        public MatrixSpecAttribute(ServerServerApiVersion supportedVer, string path,
            ServerServerApiVersion lastVersion = ServerServerApiVersion.Unknown)
        {
            Path = path;
            _apiVersionContext = new ServerServer(supportedVer, lastVersion);
        }

        public MatrixSpecAttribute(ApplicationServiceApiVersion supportedVer, string path,
            ApplicationServiceApiVersion lastVersion = ApplicationServiceApiVersion.Unknown)
        {
            Path = path;
            _apiVersionContext = new ApplicationService(supportedVer, lastVersion);
        }

        public MatrixSpecAttribute(IdentityServiceApiVersion supportedVer, string path,
            IdentityServiceApiVersion lastVersion = IdentityServiceApiVersion.Unknown)
        {
            Path = path;
            _apiVersionContext = new IdentityService(supportedVer, lastVersion);
        }

        public MatrixSpecAttribute(PushGatewayApiVersion supportedVer, string path,
            PushGatewayApiVersion lastVersion = PushGatewayApiVersion.Unknown)
        {
            Path = path;
            _apiVersionContext = new PushGateway(supportedVer, lastVersion);
        }

        public MatrixSpecAttribute(RoomsApiVersion supportedVer, string path,
            RoomsApiVersion lastVersion = RoomsApiVersion.V1)
        {
            Path = path;
            _apiVersionContext = new Rooms(supportedVer, lastVersion);
        }

        public override string ToString()
        {
            var verStr = _apiVersionContext.Api switch
                {
                MatrixSpecApi.ClientServer =>
                (_apiVersionContext as ClientServer)?
                .LastVersion.ToJsonString(),
                MatrixSpecApi.ServerServer =>
                (_apiVersionContext as ServerServer)?
                .LastVersion.ToJsonString(),
                MatrixSpecApi.ApplicationService =>
                (_apiVersionContext as ApplicationService)?
                .LastVersion.ToJsonString(),
                MatrixSpecApi.IdentityService =>
                (_apiVersionContext as IdentityService)?
                .LastVersion.ToJsonString(),
                MatrixSpecApi.PushGateway =>
                (_apiVersionContext as PushGateway)?
                .LastVersion.ToJsonString(),
                MatrixSpecApi.Rooms =>
                (_apiVersionContext as Rooms)?
                .LastVersion.ToJsonString(),
                };

            var apiStr = _apiVersionContext.Api.ToJsonString();

            return $"{MatrixSpecUrl}/{apiStr}/{verStr}.html#${Path}";
        }
    }

    public enum MatrixSpecApi
    {
        ClientServer,
        ServerServer,
        ApplicationService,
        IdentityService,
        PushGateway,
        Rooms
    }

    public static class MatrixSpecApiExtensions
    {
        public static string ToJsonString(this MatrixSpecApi clientServerApiVersion)
        {
            return clientServerApiVersion switch
                {
                MatrixSpecApi.ClientServer => @"client_server",
                MatrixSpecApi.ServerServer => @"server_server",
                MatrixSpecApi.ApplicationService => @"application_service",
                MatrixSpecApi.IdentityService => @"identity_service",
                MatrixSpecApi.PushGateway => @"push_gateway",
                MatrixSpecApi.Rooms => @"rooms"
                };
        }

        public static MatrixSpecApi FromJsonString(this string clientServerApiVersion)
        {
            return clientServerApiVersion switch
                {
                @"client_server" => MatrixSpecApi.ClientServer,
                @"server_server" => MatrixSpecApi.ServerServer,
                @"application_service" => MatrixSpecApi.ApplicationService,
                @"identity_service" => MatrixSpecApi.IdentityService,
                @"push_gateway" => MatrixSpecApi.PushGateway,
                @"rooms" => MatrixSpecApi.Rooms
                };
        }
    }

    /// <summary>
    /// The versions this method
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property |
                    AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field)]
    public class MatrixSpecVersionAttribute : Attribute
    {
        public string[] Versions { get; }

        public MatrixSpecVersionAttribute(params string[] versions)
        {
            Versions = versions;
        }
    }
}
