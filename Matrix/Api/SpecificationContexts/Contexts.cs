using System.Collections.Generic;

using Matrix.Api.Versions;

namespace Matrix.Api.SpecificationContexts
{
    internal sealed class ClientServerSpecificationContext : SpecificationContext<ClientServerApiVersion>
    {
        internal ClientServerSpecificationContext(ClientServerApiVersion addedVersion,
            ClientServerApiVersion removedVersion)
            : base(Specification.ClientServer)
        {
            AddedVersion = addedVersion;
            RemovedVersion = removedVersion;
        }
    }

    internal sealed class ServerServerSpecificationContext : SpecificationContext<ServerServerApiVersion>
    {
        internal ServerServerSpecificationContext(ServerServerApiVersion addedVersion,
            ServerServerApiVersion removedVersion)
            : base(Specification.ServerServer)
        {
            AddedVersion = addedVersion;
            RemovedVersion = removedVersion;
        }
    }

    internal sealed class ApplicationServiceSpecificationContext : SpecificationContext<ApplicationServiceApiVersion>
    {
        internal ApplicationServiceSpecificationContext(ApplicationServiceApiVersion addedVersion,
            ApplicationServiceApiVersion removedVersion) : base(Specification.ApplicationService)
        {
            AddedVersion = addedVersion;
            RemovedVersion = removedVersion;
        }
    }

    internal sealed class IdentityServiceSpecificationContext : SpecificationContext<IdentityServiceApiVersion>
    {
        internal IdentityServiceSpecificationContext(IdentityServiceApiVersion addedVersion,
            IdentityServiceApiVersion removedVersion) : base(Specification.IdentityService)
        {
            AddedVersion = addedVersion;
            RemovedVersion = removedVersion;
        }
    }

    internal sealed class PushGatewaySpecificationContext : SpecificationContext<PushGatewayApiVersion>
    {
        internal PushGatewaySpecificationContext(PushGatewayApiVersion addedVersion, PushGatewayApiVersion removedVersion)
            : base(Specification.PushGateway)
        {
            AddedVersion = addedVersion;
            RemovedVersion = removedVersion;
        }
    }

    internal sealed class RoomsSpecificationContext : SpecificationContext<RoomsApiVersion>
    {
        internal RoomsSpecificationContext(RoomsApiVersion addedVersion, RoomsApiVersion removedVersion)
            : base(Specification.Rooms)
        {
            AddedVersion = addedVersion;
            RemovedVersion = removedVersion;
        }
    }

    internal sealed class ClientServerVersionsSpecificationContext : VersionsSpecificationContext<ClientServerApiVersion>
    {
        internal ClientServerVersionsSpecificationContext(IEnumerable<ClientServerApiVersion> supportedVersions)
            : base(Specification.ClientServer) =>
            Versions = supportedVersions;
    }

    internal sealed class ServerServerVersionsSpecificationContext : VersionsSpecificationContext<ServerServerApiVersion>
    {
        internal ServerServerVersionsSpecificationContext(IEnumerable<ServerServerApiVersion> supportedVersions)
            : base(Specification.ServerServer) =>
            Versions = supportedVersions;
    }

    internal sealed class
        ApplicationServiceVersionsSpecificationContext : VersionsSpecificationContext<ApplicationServiceApiVersion>
    {
        internal ApplicationServiceVersionsSpecificationContext(
            IEnumerable<ApplicationServiceApiVersion> supportedVersions)
            : base(Specification.ApplicationService) =>
            Versions = supportedVersions;
    }

    internal sealed class
        IdentityServiceVersionsSpecificationContext : VersionsSpecificationContext<IdentityServiceApiVersion>
    {
        internal IdentityServiceVersionsSpecificationContext(IEnumerable<IdentityServiceApiVersion> supportedVersions)
            : base(Specification.IdentityService) =>
            Versions = supportedVersions;
    }

    internal sealed class PushGatewayVersionsSpecificationContext : VersionsSpecificationContext<PushGatewayApiVersion>
    {
        internal PushGatewayVersionsSpecificationContext(IEnumerable<PushGatewayApiVersion> supportedVersions)
            : base(Specification.PushGateway) =>
            Versions = supportedVersions;
    }

    internal sealed class RoomsVersionsSpecificationContext : VersionsSpecificationContext<RoomsApiVersion>
    {
        internal RoomsVersionsSpecificationContext(IEnumerable<RoomsApiVersion> supportedVersions)
            : base(Specification.ClientServer) =>
            Versions = supportedVersions;
    }
}