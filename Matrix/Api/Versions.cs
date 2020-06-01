using System;

namespace Matrix.Api
{
    namespace Versions
    {
        public enum ClientServerVersion
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

        public enum ServerServerVersion
        {
            Unknown,
            Unstable,
            R010,
            R011,
            R012,
            R013
        }

        public enum ApplicationServiceVersion
        {
            Unknown,
            Unstable,
            R010,
            R011,
            R012
        }

        public enum IdentityServiceVersion
        {
            Unknown,
            Unstable,
            R010,
            R020,
            R021,
            R030
        }

        public enum PushGatewayVersion
        {
            Unknown,
            Unstable,
            R010,
            R011
        }

        public enum RoomsVersion
        {
            V1,
            V2,
            V3,
            V4,
            V5
        }

        public static class VersionExtensions
        {
            public static string ToJsonString(this ClientServerVersion clientServerVersion)
            {
                return clientServerVersion switch
                    {
                    ClientServerVersion.Unknown => @"unknown",
                    ClientServerVersion.Unstable => @"unstable",
                    ClientServerVersion.R000 => @"r0.0.0",
                    ClientServerVersion.R001 => @"r0.0.1",
                    ClientServerVersion.R010 => @"r0.1.0",
                    ClientServerVersion.R020 => @"r0.2.0",
                    ClientServerVersion.R030 => @"r0.3.0",
                    ClientServerVersion.R040 => @"r0.4.0",
                    ClientServerVersion.R050 => @"r0.5.0",
                    ClientServerVersion.R060 => @"r0.6.0",
                    _ => throw new InvalidCastException()
                    };
            }

            public static string ToJsonString(this ServerServerVersion serverServerVersion)
            {
                return serverServerVersion switch
                    {
                    ServerServerVersion.Unknown => @"unknown",
                    ServerServerVersion.Unstable => @"unstable",
                    ServerServerVersion.R010 => @"r0.1.0",
                    ServerServerVersion.R011 => @"r0.1.1",
                    ServerServerVersion.R012 => @"r0.1.2",
                    ServerServerVersion.R013 => @"r0.1.3",
                    _ => throw new InvalidCastException()
                    };
            }

            public static string ToJsonString(this ApplicationServiceVersion applicationServiceVersion)
            {
                return applicationServiceVersion switch
                    {
                    ApplicationServiceVersion.Unknown => @"unknown",
                    ApplicationServiceVersion.Unstable => @"unstable",
                    ApplicationServiceVersion.R010 => @"r0.1.0",
                    ApplicationServiceVersion.R011 => @"r0.1.1",
                    ApplicationServiceVersion.R012 => @"r0.1.2",
                    _ => throw new InvalidCastException()
                    };
            }

            public static string ToJsonString(this IdentityServiceVersion identityServiceVersion)
            {
                return identityServiceVersion switch
                    {
                    IdentityServiceVersion.Unknown => @"unknown",
                    IdentityServiceVersion.Unstable => @"unstable",
                    IdentityServiceVersion.R010 => @"r0.1.0",
                    IdentityServiceVersion.R020 => @"r0.2.0",
                    IdentityServiceVersion.R021 => @"r0.2.1",
                    IdentityServiceVersion.R030 => @"r0.3.0",
                    _ => throw new InvalidCastException()
                    };
            }

            public static string ToJsonString(this PushGatewayVersion pushGatewayVersion)
            {
                return pushGatewayVersion switch
                    {
                    PushGatewayVersion.Unknown => @"unknown",
                    PushGatewayVersion.Unstable => @"unstable",
                    PushGatewayVersion.R010 => @"r0.1.0",
                    PushGatewayVersion.R011 => @"r0.1.1",
                    _ => throw new InvalidCastException()
                    };
            }

            public static string ToJsonString(this RoomsVersion roomsVersion)
            {
                return roomsVersion switch
                    {
                    RoomsVersion.V1 => @"v1",
                    RoomsVersion.V2 => @"v2",
                    RoomsVersion.V3 => @"v3",
                    RoomsVersion.V4 => @"v4",
                    RoomsVersion.V5 => @"v5",
                    _ => throw new InvalidCastException()
                    };
            }

            public static ClientServerVersion ToClientServerVersion(this string clientServerVersion)
            {
                return clientServerVersion switch
                    {
                    @"unknown" => ClientServerVersion.Unknown,
                    @"unstable" => ClientServerVersion.Unstable,
                    @"r0.0.0" => ClientServerVersion.R000,
                    @"r0.0.1" => ClientServerVersion.R001,
                    @"r0.1.0" => ClientServerVersion.R010,
                    @"r0.2.0" => ClientServerVersion.R020,
                    @"r0.3.0" => ClientServerVersion.R030,
                    @"r0.4.0" => ClientServerVersion.R040,
                    @"r0.5.0" => ClientServerVersion.R050,
                    @"r0.6.0" => ClientServerVersion.R060,
                    _ => throw new InvalidCastException()
                    };
            }

            public static ServerServerVersion ToServerServerVersion(this string serverServerVersion)
            {
                return serverServerVersion switch
                    {
                    @"unknown" => ServerServerVersion.Unknown,
                    @"unstable" => ServerServerVersion.Unstable,
                    @"r0.1.0" => ServerServerVersion.R010,
                    @"r0.1.1" => ServerServerVersion.R011,
                    @"r0.1.2" => ServerServerVersion.R012,
                    @"r0.1.3" => ServerServerVersion.R013,
                    _ => throw new InvalidCastException()
                    };
            }

            public static ApplicationServiceVersion ToApplicationServiceVersion(this string applicationServiceVersion)
            {
                return applicationServiceVersion switch
                    {
                    @"unknown" => ApplicationServiceVersion.Unknown,
                    @"unstable" => ApplicationServiceVersion.Unstable,
                    @"r0.1.0" => ApplicationServiceVersion.R010,
                    @"r0.1.1" => ApplicationServiceVersion.R011,
                    @"r0.1.2" => ApplicationServiceVersion.R012,
                    _ => throw new InvalidCastException()
                    };
            }

            public static IdentityServiceVersion ToIdentityServiceVersion(this string identityServiceVersion)
            {
                return identityServiceVersion switch
                    {
                    @"unknown" => IdentityServiceVersion.Unknown,
                    @"unstable" => IdentityServiceVersion.Unstable,
                    @"r0.1.0" => IdentityServiceVersion.R010,
                    @"r0.2.0" => IdentityServiceVersion.R020,
                    @"r0.2.1" => IdentityServiceVersion.R021,
                    @"r0.3.0" => IdentityServiceVersion.R030,
                    _ => throw new InvalidCastException()
                    };
            }

            public static PushGatewayVersion ToPushGatewayVersion(this string pushGatewayVersion)
            {
                return pushGatewayVersion switch
                    {
                    @"unknown" => PushGatewayVersion.Unknown,
                    @"unstable" => PushGatewayVersion.Unstable,
                    @"r0.1.0" => PushGatewayVersion.R010,
                    @"r0.1.1" => PushGatewayVersion.R011,
                    _ => throw new InvalidCastException()
                    };
            }

            public static RoomsVersion ToRoomsVersion(this string roomVersion)
            {
                return roomVersion switch
                    {
                    @"1" => RoomsVersion.V1,
                    @"2" => RoomsVersion.V2,
                    @"3" => RoomsVersion.V3,
                    @"4" => RoomsVersion.V4,
                    @"5" => RoomsVersion.V5,
                    _ => throw new InvalidCastException()
                    };
            }
        }
    }
}
