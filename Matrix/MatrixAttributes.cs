using System;

namespace Matrix
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MatrixSpecAttribute : Attribute
    {
        private const string MatrixSpecUrl = "http://matrix.org/docs/spec/";

        public EMatrixSpecApiVersion MinVersion { get; }
        public EMatrixSpecApiVersion LastVersion { get; }
        public EMatrixSpecApi Api { get; }
        public string Path { get; }

        public MatrixSpecAttribute(EMatrixSpecApiVersion supportedVer, EMatrixSpecApi api,
            string path, EMatrixSpecApiVersion lastVersion = EMatrixSpecApiVersion.Unknown)
        {
            Api = api;
            Path = path;
            MinVersion = supportedVer;
            LastVersion = lastVersion;
        }

        public override string ToString()
        {
            var verStr = GetStringForVersion(LastVersion);
            var apiStr = Api switch
            {
                EMatrixSpecApi.ClientServer => "client_server",
                EMatrixSpecApi.ApplicationService => "application_service",
                _ => "ERROR UNKNOWN API TYPE",
            };
            return $"{MatrixSpecUrl}/{apiStr}/{verStr}.html#${Path}";
        }

        public static string GetStringForVersion(EMatrixSpecApiVersion version)
        {
            return version switch
            {
                EMatrixSpecApiVersion.R001 => "r0.0.1",
                EMatrixSpecApiVersion.R010 => "r0.1.0",
                EMatrixSpecApiVersion.R020 => "r0.2.0",
                EMatrixSpecApiVersion.R030 => "r0.3.0",
                EMatrixSpecApiVersion.R040 => "r0.4.0",
                _ => "unstable",
            };
        }

        public static EMatrixSpecApiVersion GetVersionForString(string version)
        {
            return version switch
            {
                "r0.0.1" => EMatrixSpecApiVersion.R001,
                "r0.1.0" => EMatrixSpecApiVersion.R010,
                "r0.2.0" => EMatrixSpecApiVersion.R020,
                "r0.3.0" => EMatrixSpecApiVersion.R030,
                "r0.4.0" => EMatrixSpecApiVersion.R040,
                _ => EMatrixSpecApiVersion.Unknown,
            };
        }
    }

    public enum EMatrixSpecApi
    {
        ClientServer,
        ApplicationService
    }

    public enum EMatrixSpecApiVersion
    {
        Unknown,
        Unstable,
        R001,
        R010,
        R020,
        R030,
        R040
    }

    /// <summary>
    /// The versions this method
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property |
                    AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field)]
    public class MatrixSpecVersionAttribute : Attribute
    {
        public string[] Versions { get; }

        public MatrixSpecVersionAttribute(params string[] versions)
        {
            Versions = versions;
        }
    }
}