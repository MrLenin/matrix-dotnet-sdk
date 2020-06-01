using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using Matrix.Api.Versions;
using Matrix.Properties;

namespace Matrix.Api
{
    internal abstract class SpecContext
    {
        protected internal Specification Specification { get; }

        protected SpecContext(Specification specification)
        {
            Specification = specification;
        }

        internal static SpecContext<ClientServerVersion> ClientServer(SpecContext baseContext)
        {
            return baseContext as SpecContext<ClientServerVersion> ??
                   throw new ArgumentException(Resources.InvalidClientServerVersionContext);
        }

        internal static SpecContext<ServerServerVersion> ServerServer(SpecContext baseContext)
        {
            return baseContext as SpecContext<ServerServerVersion> ??
                   throw new ArgumentException(Resources.InvalidServerServerVersionContext);
        }

        internal static SpecContext<ApplicationServiceVersion> ApplicationService(
            SpecContext baseContext)
        {
            return baseContext as SpecContext<ApplicationServiceVersion> ??
                   throw new ArgumentException(Resources.InvalidApplicationServiceVersionContext);
        }

        internal static SpecContext<IdentityServiceVersion> IdentityService(SpecContext baseContext)
        {
            return baseContext as SpecContext<IdentityServiceVersion> ??
                   throw new ArgumentException(Resources.InvalidIdentityServiceVersionContext);
        }

        internal static SpecContext<PushGatewayVersion> PushGateway(SpecContext baseContext)
        {
            return baseContext as SpecContext<PushGatewayVersion> ??
                   throw new ArgumentException(Resources.InvalidPushGatewayVersionContext);
        }

        internal static SpecContext<RoomsVersion> Rooms(SpecContext baseContext)
        {
            return baseContext as SpecContext<RoomsVersion> ??
                   throw new ArgumentException(Resources.InvalidRoomsVersionContext);
        }

        internal static VersionsSpecContext<ClientServerVersion> ClientServerVersions(
            SpecContext baseContext)
        {
            return baseContext as VersionsSpecContext<ClientServerVersion> ??
                   throw new ArgumentException(Resources.InvalidClientServerVersionListContext);
        }

        internal static VersionsSpecContext<ServerServerVersion> ServerServerVersions(
            SpecContext baseContext)
        {
            return baseContext as VersionsSpecContext<ServerServerVersion> ??
                   throw new ArgumentException(Resources.InvalidServerServerVersionListContext);
        }

        internal static VersionsSpecContext<ApplicationServiceVersion> ApplicationServiceVersions(
            SpecContext baseContext)
        {
            return baseContext as VersionsSpecContext<ApplicationServiceVersion> ??
                   throw new ArgumentException(Resources.InvalidApplicationServiceVersionListContext);
        }

        internal static VersionsSpecContext<IdentityServiceVersion> IdentityServiceVersions(
            SpecContext baseContext)
        {
            return baseContext as VersionsSpecContext<IdentityServiceVersion> ??
                   throw new ArgumentException(Resources.InvalidIdentityServiceVersionListContext);
        }

        internal static VersionsSpecContext<PushGatewayVersion> PushGatewayVersions(
            SpecContext baseContext)
        {
            return baseContext as VersionsSpecContext<PushGatewayVersion> ??
                   throw new ArgumentException(Resources.InvalidPushGatewayVersionListContext);
        }

        internal static VersionsSpecContext<RoomsVersion> RoomsVersions(SpecContext baseContext)
        {
            return baseContext as VersionsSpecContext<RoomsVersion> ??
                   throw new ArgumentException(Resources.InvalidRoomsVersionListContext);
        }
    }

    internal class SpecContext<TVersions> : SpecContext where TVersions : Enum
    {
        internal TVersions AddedVersion { get; set; }
        internal TVersions RemovedVersion { get; set; }

        internal SpecContext(Specification specification, TVersions addedVersion, TVersions removedVersion) :
            base(specification)
        {
            AddedVersion = addedVersion;
            RemovedVersion = removedVersion;
        }
    }

    internal class VersionsSpecContext<TVersions> : SpecContext where TVersions : Enum
    {
        internal IEnumerable<TVersions> Versions { get; set; }

        internal VersionsSpecContext(Specification specification, IEnumerable<TVersions> supportedVersions) :
            base(specification)
        {
            Versions = Array.Empty<TVersions>();
        }
    }
}
