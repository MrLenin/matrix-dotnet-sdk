using System;

namespace Matrix.Api.Versions
{
    public enum ClientServerApiVersion
    {
        Unknown,
        Unstable,
        R000,
        R001,
        R010,
        R020,
        R030,
        R040,
        R050,
        R060
    }

    public enum ServerServerApiVersion
    {
        Unknown,
        Unstable,
        R010,
        R011,
        R012,
        R013
    }

    public enum ApplicationServiceApiVersion
    {
        Unknown,
        Unstable,
        R010,
        R011,
        R012
    }

    public enum IdentityServiceApiVersion
    {
        Unknown,
        Unstable,
        R010,
        R020,
        R021,
        R030
    }

    public enum PushGatewayApiVersion
    {
        Unknown,
        Unstable,
        R010,
        R011
    }

    public enum RoomsApiVersion
    {
        V1,
        V2,
        V3,
        V4,
        V5
    }

    public static class VersionExtensions
    {
        public static string ToJsonString(this ClientServerApiVersion clientServerApiVersion)
        {
            return clientServerApiVersion switch
            {
                ClientServerApiVersion.Unknown => @"unknown",
                ClientServerApiVersion.Unstable => @"unstable",
                ClientServerApiVersion.R000 => @"r0.0.0",
                ClientServerApiVersion.R001 => @"r0.0.1",
                ClientServerApiVersion.R010 => @"r0.1.0",
                ClientServerApiVersion.R020 => @"r0.2.0",
                ClientServerApiVersion.R030 => @"r0.3.0",
                ClientServerApiVersion.R040 => @"r0.4.0",
                ClientServerApiVersion.R050 => @"r0.5.0",
                ClientServerApiVersion.R060 => @"r0.6.0",
                _ => throw new System.NotImplementedException()
            };
        }

        public static string ToJsonString(this ServerServerApiVersion serverServerApiVersion)
        {
            return serverServerApiVersion switch
            {
                ServerServerApiVersion.Unknown => @"unknown",
                ServerServerApiVersion.Unstable => @"unstable",
                ServerServerApiVersion.R010 => @"r0.1.0",
                ServerServerApiVersion.R011 => @"r0.1.1",
                ServerServerApiVersion.R012 => @"r0.1.2",
                ServerServerApiVersion.R013 => @"r0.1.3",
                _ => throw new System.NotImplementedException()
            };
        }

        public static string ToJsonString(this ApplicationServiceApiVersion applicationServiceApiVersion)
        {
            return applicationServiceApiVersion switch
            {
                ApplicationServiceApiVersion.Unknown => @"unknown",
                ApplicationServiceApiVersion.Unstable => @"unstable",
                ApplicationServiceApiVersion.R010 => @"r0.1.0",
                ApplicationServiceApiVersion.R011 => @"r0.1.1",
                ApplicationServiceApiVersion.R012 => @"r0.1.2",
                _ => throw new System.NotImplementedException()
            };
        }

        public static string ToJsonString(this IdentityServiceApiVersion identityServiceApiVersion)
        {
            return identityServiceApiVersion switch
            {
                IdentityServiceApiVersion.Unknown => @"unknown",
                IdentityServiceApiVersion.Unstable => @"unstable",
                IdentityServiceApiVersion.R010 => @"r0.1.0",
                IdentityServiceApiVersion.R020 => @"r0.2.0",
                IdentityServiceApiVersion.R021 => @"r0.2.1",
                IdentityServiceApiVersion.R030 => @"r0.3.0",
                _ => throw new System.NotImplementedException()
            };
        }

        public static string ToJsonString(this PushGatewayApiVersion pushGatewayApiVersion)
        {
            return pushGatewayApiVersion switch
            {
                PushGatewayApiVersion.Unknown => @"unknown",
                PushGatewayApiVersion.Unstable => @"unstable",
                PushGatewayApiVersion.R010 => @"r0.1.0",
                PushGatewayApiVersion.R011 => @"r0.1.1",
                _ => throw new System.NotImplementedException()
            };
        }

        public static string ToJsonString(this RoomsApiVersion roomsApiVersion)
        {
            return roomsApiVersion switch
            {
                RoomsApiVersion.V1 => @"v1",
                RoomsApiVersion.V2 => @"v2",
                RoomsApiVersion.V3 => @"v3",
                RoomsApiVersion.V4 => @"v4",
                RoomsApiVersion.V5 => @"v5",
                _ => throw new System.NotImplementedException()
            };
        }

        public static ClientServerApiVersion ToClientServerApiVersion(this string clientServerApiVersion)
        {
            return clientServerApiVersion switch
            {
                @"unknown" => ClientServerApiVersion.Unknown,
                @"unstable" => ClientServerApiVersion.Unstable,
                @"r0.0.0" => ClientServerApiVersion.R000,
                @"r0.0.1" => ClientServerApiVersion.R001,
                @"r0.1.0" => ClientServerApiVersion.R010,
                @"r0.2.0" => ClientServerApiVersion.R020,
                @"r0.3.0" => ClientServerApiVersion.R030,
                @"r0.4.0" => ClientServerApiVersion.R040,
                @"r0.5.0" => ClientServerApiVersion.R050,
                @"r0.6.0" => ClientServerApiVersion.R060,
                _ => throw new System.NotImplementedException()
            };
        }

        public static ServerServerApiVersion ToServerServerApiVersion(this string serverServerApiVersion)
        {
            return serverServerApiVersion switch
            {
                @"unknown" => ServerServerApiVersion.Unknown,
                @"unstable" => ServerServerApiVersion.Unstable,
                @"r0.1.0" => ServerServerApiVersion.R010,
                @"r0.1.1" => ServerServerApiVersion.R011,
                @"r0.1.2" => ServerServerApiVersion.R012,
                @"r0.1.3" => ServerServerApiVersion.R013,
                _ => throw new System.NotImplementedException()
            };
        }

        public static ApplicationServiceApiVersion ToApplicationServiceApiVersion(this string applicationServiceApiVersion)
        {
            return applicationServiceApiVersion switch
            {
                @"unknown" => ApplicationServiceApiVersion.Unknown,
                @"unstable" => ApplicationServiceApiVersion.Unstable,
                @"r0.1.0" => ApplicationServiceApiVersion.R010,
                @"r0.1.1" => ApplicationServiceApiVersion.R011,
                @"r0.1.2" => ApplicationServiceApiVersion.R012,
                _ => throw new System.NotImplementedException()
            };
        }

        public static IdentityServiceApiVersion ToIdentityServiceApiVersion(this string identityServiceApiVersion)
        {
            return identityServiceApiVersion switch
            {
                @"unknown" => IdentityServiceApiVersion.Unknown,
                @"unstable" => IdentityServiceApiVersion.Unstable,
                @"r0.1.0" => IdentityServiceApiVersion.R010,
                @"r0.2.0" => IdentityServiceApiVersion.R020,
                @"r0.2.1" => IdentityServiceApiVersion.R021,
                @"r0.3.0" => IdentityServiceApiVersion.R030,
                _ => throw new System.NotImplementedException()
            };
        }

        public static PushGatewayApiVersion ToPushGatewayApiVersion(this string pushGatewayApiVersion)
        {
            return pushGatewayApiVersion switch
            {
                @"unknown" => PushGatewayApiVersion.Unknown,
                @"unstable" => PushGatewayApiVersion.Unstable,
                @"r0.1.0" => PushGatewayApiVersion.R010,
                @"r0.1.1" => PushGatewayApiVersion.R011,
                _ => throw new System.NotImplementedException()
            };
        }

        public static RoomsApiVersion ToRoomsApiVersion(this string roomApiVersion)
        {
            return roomApiVersion switch
            {
                @"v1" => RoomsApiVersion.V1,
                @"v2" => RoomsApiVersion.V2,
                @"v3" => RoomsApiVersion.V3,
                @"v4" => RoomsApiVersion.V4,
                @"v5" => RoomsApiVersion.V5,
                _ => throw new System.NotImplementedException()
            };
        }
    }
}
