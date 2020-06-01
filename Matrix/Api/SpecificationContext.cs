using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using Matrix.Api.Versions;
using Matrix.Properties;

namespace Matrix.Api
{
    internal abstract class SpecificationContext
    {
        protected internal Specification Specification { get; }

        protected SpecificationContext(Specification specification)
        {
            Specification = specification;
        }

        internal static SpecificationContext<ClientServerVersion> ClientServer(SpecificationContext baseContext)
        {
            return baseContext as SpecificationContext<ClientServerVersion> ??
                   throw new ArgumentException(Resources.InvalidClientServerVersionContext);
        }

        internal static SpecificationContext<ServerServerVersion> ServerServer(SpecificationContext baseContext)
        {
            return baseContext as SpecificationContext<ServerServerVersion> ??
                   throw new ArgumentException(Resources.InvalidServerServerVersionContext);
        }

        internal static SpecificationContext<ApplicationServiceVersion> ApplicationService(
            SpecificationContext baseContext)
        {
            return baseContext as SpecificationContext<ApplicationServiceVersion> ??
                   throw new ArgumentException(Resources.InvalidApplicationServiceVersionContext);
        }

        internal static SpecificationContext<IdentityServiceVersion> IdentityService(SpecificationContext baseContext)
        {
            return baseContext as SpecificationContext<IdentityServiceVersion> ??
                   throw new ArgumentException(Resources.InvalidIdentityServiceVersionContext);
        }

        internal static SpecificationContext<PushGatewayVersion> PushGateway(SpecificationContext baseContext)
        {
            return baseContext as SpecificationContext<PushGatewayVersion> ??
                   throw new ArgumentException(Resources.InvalidPushGatewayVersionContext);
        }

        internal static SpecificationContext<RoomsVersion> Rooms(SpecificationContext baseContext)
        {
            return baseContext as SpecificationContext<RoomsVersion> ??
                   throw new ArgumentException(Resources.InvalidRoomsVersionContext);
        }

        internal static VersionsSpecificationContext<ClientServerVersion> ClientServerVersions(
            SpecificationContext baseContext)
        {
            return baseContext as VersionsSpecificationContext<ClientServerVersion> ??
                   throw new ArgumentException(Resources.InvalidClientServerVersionListContext);
        }

        internal static VersionsSpecificationContext<ServerServerVersion> ServerServerVersions(
            SpecificationContext baseContext)
        {
            return baseContext as VersionsSpecificationContext<ServerServerVersion> ??
                   throw new ArgumentException(Resources.InvalidServerServerVersionListContext);
        }

        internal static VersionsSpecificationContext<ApplicationServiceVersion> ApplicationServiceVersions(
            SpecificationContext baseContext)
        {
            return baseContext as VersionsSpecificationContext<ApplicationServiceVersion> ??
                   throw new ArgumentException(Resources.InvalidApplicationServiceVersionListContext);
        }

        internal static VersionsSpecificationContext<IdentityServiceVersion> IdentityServiceVersions(
            SpecificationContext baseContext)
        {
            return baseContext as VersionsSpecificationContext<IdentityServiceVersion> ??
                   throw new ArgumentException(Resources.InvalidIdentityServiceVersionListContext);
        }

        internal static VersionsSpecificationContext<PushGatewayVersion> PushGatewayVersions(
            SpecificationContext baseContext)
        {
            return baseContext as VersionsSpecificationContext<PushGatewayVersion> ??
                   throw new ArgumentException(Resources.InvalidPushGatewayVersionListContext);
        }

        internal static VersionsSpecificationContext<RoomsVersion> RoomsVersions(SpecificationContext baseContext)
        {
            return baseContext as VersionsSpecificationContext<RoomsVersion> ??
                   throw new ArgumentException(Resources.InvalidRoomsVersionListContext);
        }
    }

    internal class SpecificationContext<TVersions> : SpecificationContext where TVersions : Enum
    {
        internal TVersions AddedVersion { get; set; }
        internal TVersions RemovedVersion { get; set; }

        internal SpecificationContext(Specification specification, TVersions addedVersion, TVersions removedVersion) :
            base(specification)
        {
            AddedVersion = addedVersion;
            RemovedVersion = removedVersion;
        }
    }

    internal class VersionsSpecificationContext<TVersions> : SpecificationContext where TVersions : Enum
    {
        internal IEnumerable<TVersions> Versions { get; set; }

        internal VersionsSpecificationContext(Specification specification, IEnumerable<TVersions> supportedVersions) :
            base(specification)
        {
            Versions = Array.Empty<TVersions>();
        }
    }
}
