using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;

namespace Matrix.AppService
{
	public struct AppServiceNamespace
	{
		public bool Exclusive  { get; set; }
		public string Regex  { get; set; }
    }

	public struct ServiceRegistrationOptionsNamespaces{
		public List<AppServiceNamespace> Users  { get; set; }
		public List<AppServiceNamespace> Aliases  { get; set; }
		public List<AppServiceNamespace> Rooms  { get; set; }
	}

	public struct ServiceRegistrationOptions{
		public string Id { get; set; }
		public Uri Url { get; set; }
		public string AppserviceToken { get; set; }
		public string HomeserverToken { get; set; }
		public string SenderLocalpart { get; set; }
		public ServiceRegistrationOptionsNamespaces Namespaces { get; set; }
	}

	public class ServiceRegistration
	{

		public Uri Url
		{
			get;
			private set;
		}

		public string Localpart
        {
			get;
			private set;
		}

		public string Id 
        {
			get;
			private set;
		}

		public string HomeserverToken 
        {
			get;
			private set;
		}

		public string AppserviceToken
        {
			get;
			private set;
		}

		public ICollection<AppServiceNamespace> NamespacesUsers
        {
			get;
			private set;
		}

		public ICollection<AppServiceNamespace> NamespacesAliases
        {
			get;
			private set;
		}

        public ICollection<AppServiceNamespace> NamespacesRooms
        {
			get;
			private set;
		}

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

		public static string GenerateToken ()
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
