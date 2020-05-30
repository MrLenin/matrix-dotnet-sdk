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

        public MatrixSpecAttribute(ClientServerApiVersion addedVersion, string path,
            ClientServerApiVersion removedVersion = ClientServerApiVersion.Unknown)
        {
            Path = path;
            SpecificationContext = new ClientServerSpecificationContext(addedVersion, removedVersion);
        }

        public MatrixSpecAttribute(ServerServerApiVersion addedVersion, string path,
            ServerServerApiVersion removedVersion = ServerServerApiVersion.Unknown)
        {
            Path = path;
            SpecificationContext = new ServerServerSpecificationContext(addedVersion, removedVersion);
        }

        public MatrixSpecAttribute(ApplicationServiceApiVersion addedVersion, string path,
            ApplicationServiceApiVersion removedVersion = ApplicationServiceApiVersion.Unknown)
        {
            Path = path;
            SpecificationContext = new ApplicationServiceSpecificationContext(addedVersion, removedVersion);
        }

        public MatrixSpecAttribute(IdentityServiceApiVersion addedVersion, string path,
            IdentityServiceApiVersion removedVersion = IdentityServiceApiVersion.Unknown)
        {
            Path = path;
            SpecificationContext = new IdentityServiceSpecificationContext(addedVersion, removedVersion);
        }

        public MatrixSpecAttribute(PushGatewayApiVersion addedVersion, string path,
            PushGatewayApiVersion removedVersion = PushGatewayApiVersion.Unknown)
        {
            Path = path;
            SpecificationContext = new PushGatewaySpecificationContext(addedVersion, removedVersion);
        }

        public MatrixSpecAttribute(RoomsApiVersion addedVersion, string path,
            RoomsApiVersion removedVersion = RoomsApiVersion.V1)
        {
            Path = path;
            SpecificationContext = new RoomsSpecificationContext(addedVersion, removedVersion);
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

        public MatrixSpecVersionsAttribute(IEnumerable<ClientServerApiVersion> supportedVersions)
        {
            SpecificationContext = new ClientServerVersionsSpecificationContext(supportedVersions);
        }

        public MatrixSpecVersionsAttribute(IEnumerable<ServerServerApiVersion> supportedVersions)
        {
            SpecificationContext = new ServerServerVersionsSpecificationContext(supportedVersions);
        }

        public MatrixSpecVersionsAttribute(IEnumerable<ApplicationServiceApiVersion> supportedVersions)
        {
            SpecificationContext = new ApplicationServiceVersionsSpecificationContext(supportedVersions);
        }

        public MatrixSpecVersionsAttribute(IEnumerable<IdentityServiceApiVersion> supportedVersions)
        {
            SpecificationContext = new IdentityServiceVersionsSpecificationContext(supportedVersions);
        }

        public MatrixSpecVersionsAttribute(IEnumerable<PushGatewayApiVersion> supportedVersions)
        {
            SpecificationContext = new PushGatewayVersionsSpecificationContext(supportedVersions);
        }

        public MatrixSpecVersionsAttribute(IEnumerable<RoomsApiVersion> supportedVersions)
        {
            SpecificationContext = new RoomsVersionsSpecificationContext(supportedVersions);
        }
    }
}
