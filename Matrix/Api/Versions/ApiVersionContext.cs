namespace Matrix.Api.Versions
{
    internal abstract class ApiVersionContext
    {
        internal MatrixSpecApi Api { get; set; }

        internal abstract class VersionContext<T> : ApiVersionContext
        {
            public T MinVersion { get; set; }
            public T LastVersion { get; set; }
        }

        private ApiVersionContext()
        {
        }

        internal sealed class ClientServer : VersionContext<ClientServerApiVersion>
        {
            internal ClientServer(ClientServerApiVersion supportedVersion, ClientServerApiVersion lastVersion)
            {
                Api = MatrixSpecApi.ClientServer;
                MinVersion = supportedVersion;
                LastVersion = lastVersion;
            }
        }

        internal sealed class ServerServer : VersionContext<ServerServerApiVersion>
        {
            internal ServerServer(ServerServerApiVersion supportedVersion, ServerServerApiVersion lastVersion)
            {
                Api = MatrixSpecApi.ServerServer;
                MinVersion = supportedVersion;
                LastVersion = lastVersion;
            }
        }

        internal sealed class ApplicationService : VersionContext<ApplicationServiceApiVersion>
        {
            internal ApplicationService(ApplicationServiceApiVersion supportedVersion,
                ApplicationServiceApiVersion lastVersion)
            {
                Api = MatrixSpecApi.ApplicationService;
                MinVersion = supportedVersion;
                LastVersion = lastVersion;
            }
        }

        internal sealed class IdentityService : VersionContext<IdentityServiceApiVersion>
        {
            internal IdentityService(IdentityServiceApiVersion supportedVersion, IdentityServiceApiVersion lastVersion)
            {
                Api = MatrixSpecApi.IdentityService;
                MinVersion = supportedVersion;
                LastVersion = lastVersion;
            }
        }

        internal sealed class PushGateway : VersionContext<PushGatewayApiVersion>
        {
            internal PushGateway(PushGatewayApiVersion supportedVersion, PushGatewayApiVersion lastVersion)
            {
                Api = MatrixSpecApi.PushGateway;
                MinVersion = supportedVersion;
                LastVersion = lastVersion;
            }
        }

        internal sealed class Rooms : VersionContext<RoomsApiVersion>
        {
            internal Rooms(RoomsApiVersion supportedVersion, RoomsApiVersion lastVersion)
            {
                Api = MatrixSpecApi.Rooms;
                MinVersion = supportedVersion;
                LastVersion = lastVersion;
            }
        }
    }
}
