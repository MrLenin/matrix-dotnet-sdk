using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using Matrix.Api.SpecificationContexts;
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

        internal static ClientServerSpecificationContext ClientServer(SpecificationContext baseContext)
        {
            return baseContext as ClientServerSpecificationContext ??
                   throw new ArgumentException(Resources.InvalidClientServerVersionContext);
        }

        internal static ServerServerSpecificationContext ServerServer(SpecificationContext baseContext)
        {
            return baseContext as ServerServerSpecificationContext ??
                   throw new ArgumentException(Resources.InvalidServerServerVersionContext);
        }

        internal static ApplicationServiceSpecificationContext ApplicationService(SpecificationContext baseContext)
        {
            return baseContext as ApplicationServiceSpecificationContext ??
                   throw new ArgumentException(Resources.InvalidApplicationServiceVersionContext);
        }

        internal static IdentityServiceSpecificationContext IdentityService(SpecificationContext baseContext)
        {
            return baseContext as IdentityServiceSpecificationContext ??
                   throw new ArgumentException(Resources.InvalidIdentityServiceVersionContext);
        }

        internal static PushGatewaySpecificationContext PushGateway(SpecificationContext baseContext)
        {
            return baseContext as PushGatewaySpecificationContext ??
                   throw new ArgumentException(Resources.InvalidPushGatewayVersionContext);
        }

        internal static RoomsSpecificationContext Rooms(SpecificationContext baseContext)
        {
            return baseContext as RoomsSpecificationContext ??
                   throw new ArgumentException(Resources.InvalidRoomsVersionContext);
        }

        internal static ClientServerVersionsSpecificationContext ClientServerVersions(SpecificationContext baseContext)
        {
            return baseContext as ClientServerVersionsSpecificationContext ??
                   throw new ArgumentException(Resources.InvalidClientServerVersionListContext);
        }

        internal static ServerServerVersionsSpecificationContext ServerServerVersions(SpecificationContext baseContext)
        {
            return baseContext as ServerServerVersionsSpecificationContext ??
                   throw new ArgumentException(Resources.InvalidServerServerVersionListContext);
        }

        internal static ApplicationServiceVersionsSpecificationContext ApplicationServiceVersions(
            SpecificationContext baseContext)
        {
            return baseContext as ApplicationServiceVersionsSpecificationContext ??
                   throw new ArgumentException(Resources.InvalidApplicationServiceVersionListContext);
        }

        internal static IdentityServiceVersionsSpecificationContext IdentityServiceVersions(
            SpecificationContext baseContext)
        {
            return baseContext as IdentityServiceVersionsSpecificationContext ??
                   throw new ArgumentException(Resources.InvalidIdentityServiceVersionListContext);
        }

        internal static PushGatewayVersionsSpecificationContext PushGatewayVersions(SpecificationContext baseContext)
        {
            return baseContext as PushGatewayVersionsSpecificationContext ??
                   throw new ArgumentException(Resources.InvalidPushGatewayVersionListContext);
        }

        internal static RoomsVersionsSpecificationContext RoomsVersions(SpecificationContext baseContext)
        {
            return baseContext as RoomsVersionsSpecificationContext ??
                   throw new ArgumentException(Resources.InvalidRoomsVersionListContext);
        }
    }

    internal abstract class SpecificationContext<T> : SpecificationContext where T : Enum
    {
        internal T AddedVersion { get; set; }
        internal T RemovedVersion { get; set; }

        internal SpecificationContext(Specification specification) : base(specification)
        {
            AddedVersion = Activator.CreateInstance<T>();
            RemovedVersion = Activator.CreateInstance<T>();
        }
    }

    internal abstract class VersionsSpecificationContext<T> : SpecificationContext where T : Enum
    {
        internal IEnumerable<T> Versions { get; set; }

        internal VersionsSpecificationContext(Specification specification) : base(specification)
        {
            Versions = Array.Empty<T>();
        }
    }
}
