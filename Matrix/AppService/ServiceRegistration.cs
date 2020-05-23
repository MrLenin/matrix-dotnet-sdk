using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;

namespace Matrix.AppService
{
    public struct AppServiceNamespace : IEquatable<AppServiceNamespace>
    {
        public bool Exclusive { get; set; }
        public string Regex { get; set; }

        public bool Equals(AppServiceNamespace other)
        {
            return (Exclusive == other.Exclusive) && (Regex == other.Regex);
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                var other = (AppServiceNamespace) obj;
                return Equals(other);
            }
        }

        public override int GetHashCode()
        {
            return Exclusive.GetHashCode() ^ Regex.GetHashCode(StringComparison.InvariantCulture);
        }

        public static bool operator ==(AppServiceNamespace left, AppServiceNamespace right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AppServiceNamespace left, AppServiceNamespace right)
        {
            return !(left == right);
        }
    }

    public struct ServiceRegistrationOptionsNamespaces : IEquatable<ServiceRegistrationOptionsNamespaces>
    {
        public List<AppServiceNamespace> Users { get; set; }
        public List<AppServiceNamespace> Aliases { get; set; }
        public List<AppServiceNamespace> Rooms { get; set; }

         public bool Equals(ServiceRegistrationOptionsNamespaces other)
         {
             return Users == other.Users && Aliases == other.Aliases && Rooms == other.Rooms;
         }

        public override bool Equals(object obj)
        {
            if ((obj == null) || GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                var other = (ServiceRegistrationOptionsNamespaces) obj;
                return Equals(other);
            }
        }

        public override int GetHashCode()
        {
            return Users.GetHashCode() ^ Aliases.GetHashCode() ^ Rooms.GetHashCode();
        }

        public static bool operator ==(ServiceRegistrationOptionsNamespaces left, ServiceRegistrationOptionsNamespaces right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ServiceRegistrationOptionsNamespaces left, ServiceRegistrationOptionsNamespaces right)
        {
            return !(left == right);
        }
    }

    public struct ServiceRegistrationOptions : IEquatable<ServiceRegistrationOptions>
    {
        public string Id { get; set; }
        public Uri Url { get; set; }
        public string AppserviceToken { get; set; }
        public string HomeserverToken { get; set; }
        public string SenderLocalpart { get; set; }
        public ServiceRegistrationOptionsNamespaces Namespaces { get; set; }
        
        public bool Equals(ServiceRegistrationOptions other)
        {
            return Id == other.Id && Url == other.Url && AppserviceToken == other.AppserviceToken &&
                   HomeserverToken == other.HomeserverToken && SenderLocalpart == other.SenderLocalpart &&
                   Namespaces == other.Namespaces;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                var other = (ServiceRegistrationOptions) obj;
                return Equals(other);
            }
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode(StringComparison.InvariantCulture) ^ Url.GetHashCode() ^
                   AppserviceToken.GetHashCode(StringComparison.InvariantCulture) ^
                   HomeserverToken.GetHashCode(StringComparison.InvariantCulture) ^
                   SenderLocalpart.GetHashCode(StringComparison.InvariantCulture) ^
                   Namespaces.GetHashCode();
        }

        public static bool operator ==(ServiceRegistrationOptions left, ServiceRegistrationOptions right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ServiceRegistrationOptions left, ServiceRegistrationOptions right)
        {
            return !(left == right);
        }
    }

    public class ServiceRegistration
    {
        public Uri Url { get; private set; }

        public string Localpart { get; private set; }

        public string Id { get; private set; }

        public string HomeserverToken { get; private set; }

        public string AppserviceToken { get; private set; }

        public ICollection<AppServiceNamespace> NamespacesUsers { get; private set; }

        public ICollection<AppServiceNamespace> NamespacesAliases { get; private set; }

        public ICollection<AppServiceNamespace> NamespacesRooms { get; private set; }

        public ServiceRegistration(ServiceRegistrationOptions options)
        {
            Url = options.Url;
            Localpart = options.SenderLocalpart;
            Id = options.Id;
            HomeserverToken = options.HomeserverToken;
            AppserviceToken = options.AppserviceToken;
            NamespacesAliases = options.Namespaces.Aliases;
            NamespacesUsers = options.Namespaces.Users;
            NamespacesRooms = options.Namespaces.Rooms;
        }

        public ServiceRegistration(
            Uri url, string localpart, ICollection<AppServiceNamespace> users,
            ICollection<AppServiceNamespace> aliases, ICollection<AppServiceNamespace> rooms)
        {
            Url = url;
            Localpart = localpart;
            NamespacesUsers = users;
            NamespacesAliases = aliases;
            NamespacesRooms = rooms;

            Id = GenerateToken();
            HomeserverToken = GenerateToken();
            AppserviceToken = GenerateToken();
        }

        public static string GenerateToken()
        {
            return (Guid.NewGuid() + Guid.NewGuid().ToString()).Replace(
                "-", "", StringComparison.InvariantCulture);
        }

        public static ServiceRegistration FromYaml(string yaml)
        {
            var serial = new Deserializer();
            var opts = serial.Deserialize<ServiceRegistrationOptions>(new StringReader(yaml));
            return new ServiceRegistration(opts);
        }

        public string ToYaml()
        {
            var serial = new Serializer();
            using var writer = new StringWriter();
            serial.Serialize(writer, new
            {
                id = Id,
                url = Url,
                AppserviceToken,
                HomeserverToken,
                SenderLocalpart = Localpart,
                namespaces = new
                {
                    users = NamespacesUsers,
                    aliases = NamespacesAliases,
                    rooms = NamespacesRooms
                }
            });
            return writer.ToString();
        }
    }
}