using System;
using System.Collections.Generic;
using System.IO;

using Matrix.Api;
using Matrix.Api.Versions;
using Matrix.Properties;

namespace Matrix
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MatrixSpecAttribute : Attribute
    {
        private const string MatrixSpecUrl = "http://matrix.org/docs/spec/";

        internal SpecContext SpecContext { get; }

        private string Path { get; }

        public MatrixSpecAttribute(ClientServerVersion addedVersion, string path,
            ClientServerVersion removedVersion = ClientServerVersion.Unknown)
        {
            Path = path;
            SpecContext =
                new SpecContext<ClientServerVersion>(Specification.ClientServer, addedVersion, removedVersion);
        }

        public MatrixSpecAttribute(ServerServerVersion addedVersion, string path,
            ServerServerVersion removedVersion = ServerServerVersion.Unknown)
        {
            Path = path;
            SpecContext =
                new SpecContext<ServerServerVersion>(Specification.ServerServer, addedVersion, removedVersion);
        }

        public MatrixSpecAttribute(ApplicationServiceVersion addedVersion, string path,
            ApplicationServiceVersion removedVersion = ApplicationServiceVersion.Unknown)
        {
            Path = path;
            SpecContext =
                new SpecContext<ApplicationServiceVersion>(Specification.ApplicationService, addedVersion,
                    removedVersion);
        }

        public MatrixSpecAttribute(IdentityServiceVersion addedVersion, string path,
            IdentityServiceVersion removedVersion = IdentityServiceVersion.Unknown)
        {
            Path = path;
            SpecContext =
                new SpecContext<IdentityServiceVersion>(Specification.IdentityService, addedVersion,
                    removedVersion);
        }

        public MatrixSpecAttribute(PushGatewayVersion addedVersion, string path,
            PushGatewayVersion removedVersion = PushGatewayVersion.Unknown)
        {
            Path = path;
            SpecContext =
                new SpecContext<PushGatewayVersion>(Specification.PushGateway, addedVersion, removedVersion);
        }

        public MatrixSpecAttribute(RoomsVersion addedVersion, string path,
            RoomsVersion removedVersion = RoomsVersion.V1)
        {
            Path = path;
            SpecContext =
                new SpecContext<RoomsVersion>(Specification.Rooms, addedVersion, removedVersion);
        }

        public string ToJsonString()
        {
            var verStr = SpecContext.Specification switch
                {
                Specification.ClientServer =>
                SpecContext.ClientServer(SpecContext).RemovedVersion.ToJsonString(),
                Specification.ServerServer =>
                SpecContext.ServerServer(SpecContext).RemovedVersion.ToJsonString(),
                Specification.ApplicationService =>
                SpecContext.ApplicationService(SpecContext).RemovedVersion.ToJsonString(),
                Specification.IdentityService =>
                SpecContext.IdentityService(SpecContext).RemovedVersion.ToJsonString(),
                Specification.PushGateway =>
                SpecContext.PushGateway(SpecContext).RemovedVersion.ToJsonString(),
                Specification.Rooms =>
                SpecContext.Rooms(SpecContext).RemovedVersion.ToJsonString(),
                _ => throw new InvalidDataException(Resources.UnknownMatrixApiType)
                };

            var apiStr = SpecContext.Specification.ToJsonString();

            return $"{MatrixSpecUrl}/{apiStr}/{verStr}.html#${Path}";
        }
    }

    /// <summary>
    /// The versions this method
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property |
                    AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field)]
    public class MatrixSpecVersionsAttribute : Attribute
    {
        internal SpecContext SpecContext { get; }

        public MatrixSpecVersionsAttribute(IEnumerable<ClientServerVersion> supportedVersions)
        {
            SpecContext =
                new VersionsSpecContext<ClientServerVersion>(Specification.ClientServer, supportedVersions);
        }

        public MatrixSpecVersionsAttribute(IEnumerable<ServerServerVersion> supportedVersions)
        {
            SpecContext =
                new VersionsSpecContext<ServerServerVersion>(Specification.ServerServer, supportedVersions);
        }

        public MatrixSpecVersionsAttribute(IEnumerable<ApplicationServiceVersion> supportedVersions)
        {
            SpecContext =
                new VersionsSpecContext<ApplicationServiceVersion>(Specification.ApplicationService,
                    supportedVersions);
        }

        public MatrixSpecVersionsAttribute(IEnumerable<IdentityServiceVersion> supportedVersions)
        {
            SpecContext =
                new VersionsSpecContext<IdentityServiceVersion>(Specification.IdentityService,
                    supportedVersions);
        }

        public MatrixSpecVersionsAttribute(IEnumerable<PushGatewayVersion> supportedVersions)
        {
            SpecContext =
                new VersionsSpecContext<PushGatewayVersion>(Specification.PushGateway, supportedVersions);
        }

        public MatrixSpecVersionsAttribute(IEnumerable<RoomsVersion> supportedVersions)
        {
            SpecContext =
                new VersionsSpecContext<RoomsVersion>(Specification.Rooms, supportedVersions);
        }
    }
}
