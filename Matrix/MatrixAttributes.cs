using System;
using System.Collections.Generic;
using System.IO;

using Matrix.Api;
using Matrix.Api.SpecificationContexts;
using Matrix.Api.Versions;
using Matrix.Properties;

namespace Matrix
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MatrixSpecAttribute : Attribute
    {
        private const string MatrixSpecUrl = "http://matrix.org/docs/spec/";

        internal SpecificationContext SpecificationContext { get; }

        private string Path { get; }

        public MatrixSpecAttribute(ClientServerVersion addedVersion, string path,
            ClientServerVersion removedVersion = ClientServerVersion.Unknown)
        {
            Path = path;
            SpecificationContext =
                new SpecificationContext<ClientServerVersion>(Specification.ClientServer, addedVersion, removedVersion);
        }

        public MatrixSpecAttribute(ServerServerVersion addedVersion, string path,
            ServerServerVersion removedVersion = ServerServerVersion.Unknown)
        {
            Path = path;
            SpecificationContext =
                new SpecificationContext<ServerServerVersion>(Specification.ServerServer, addedVersion, removedVersion);
        }

        public MatrixSpecAttribute(ApplicationServiceVersion addedVersion, string path,
            ApplicationServiceVersion removedVersion = ApplicationServiceVersion.Unknown)
        {
            Path = path;
            SpecificationContext =
                new SpecificationContext<ApplicationServiceVersion>(Specification.ApplicationService, addedVersion,
                    removedVersion);
        }

        public MatrixSpecAttribute(IdentityServiceVersion addedVersion, string path,
            IdentityServiceVersion removedVersion = IdentityServiceVersion.Unknown)
        {
            Path = path;
            SpecificationContext =
                new SpecificationContext<IdentityServiceVersion>(Specification.IdentityService, addedVersion,
                    removedVersion);
        }

        public MatrixSpecAttribute(PushGatewayVersion addedVersion, string path,
            PushGatewayVersion removedVersion = PushGatewayVersion.Unknown)
        {
            Path = path;
            SpecificationContext =
                new SpecificationContext<PushGatewayVersion>(Specification.PushGateway, addedVersion, removedVersion);
        }

        public MatrixSpecAttribute(RoomsVersion addedVersion, string path,
            RoomsVersion removedVersion = RoomsVersion.V1)
        {
            Path = path;
            SpecificationContext =
                new SpecificationContext<RoomsVersion>(Specification.Rooms, addedVersion, removedVersion);
        }

        public string ToJsonString()
        {
            var verStr = SpecificationContext.Specification switch
                {
                Specification.ClientServer =>
                SpecificationContext.ClientServer(SpecificationContext).RemovedVersion.ToJsonString(),
                Specification.ServerServer =>
                SpecificationContext.ServerServer(SpecificationContext).RemovedVersion.ToJsonString(),
                Specification.ApplicationService =>
                SpecificationContext.ApplicationService(SpecificationContext).RemovedVersion.ToJsonString(),
                Specification.IdentityService =>
                SpecificationContext.IdentityService(SpecificationContext).RemovedVersion.ToJsonString(),
                Specification.PushGateway =>
                SpecificationContext.PushGateway(SpecificationContext).RemovedVersion.ToJsonString(),
                Specification.Rooms =>
                SpecificationContext.Rooms(SpecificationContext).RemovedVersion.ToJsonString(),
                _ => throw new InvalidDataException(Resources.UnknownMatrixApiType)
                };

            var apiStr = SpecificationContext.Specification.ToJsonString();

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
        internal SpecificationContext SpecificationContext { get; }

        public MatrixSpecVersionsAttribute(IEnumerable<ClientServerVersion> supportedVersions)
        {
            SpecificationContext =
                new VersionsSpecificationContext<ClientServerVersion>(Specification.ClientServer, supportedVersions);
        }

        public MatrixSpecVersionsAttribute(IEnumerable<ServerServerVersion> supportedVersions)
        {
            SpecificationContext =
                new VersionsSpecificationContext<ServerServerVersion>(Specification.ServerServer, supportedVersions);
        }

        public MatrixSpecVersionsAttribute(IEnumerable<ApplicationServiceVersion> supportedVersions)
        {
            SpecificationContext =
                new VersionsSpecificationContext<ApplicationServiceVersion>(Specification.ApplicationService,
                    supportedVersions);
        }

        public MatrixSpecVersionsAttribute(IEnumerable<IdentityServiceVersion> supportedVersions)
        {
            SpecificationContext =
                new VersionsSpecificationContext<IdentityServiceVersion>(Specification.IdentityService,
                    supportedVersions);
        }

        public MatrixSpecVersionsAttribute(IEnumerable<PushGatewayVersion> supportedVersions)
        {
            SpecificationContext =
                new VersionsSpecificationContext<PushGatewayVersion>(Specification.PushGateway, supportedVersions);
        }

        public MatrixSpecVersionsAttribute(IEnumerable<RoomsVersion> supportedVersions)
        {
            SpecificationContext =
                new VersionsSpecificationContext<RoomsVersion>(Specification.Rooms, supportedVersions);
        }
    }
}
