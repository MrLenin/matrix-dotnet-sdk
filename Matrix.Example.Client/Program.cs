using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Matrix.Client;
using Matrix.Structures;

namespace Matrix.Example.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var homeserverUrl = new Uri("http://localhost:8008");

            MatrixClient client;
            if (File.Exists("/tmp/mx_access"))
            {
                var tokens = File.ReadAllText("/tmp/mx_access").Split("$");
                client = new MatrixClient(homeserverUrl);
                client.UseExistingToken(tokens[1], tokens[0]);
            }
            else
            {
                var username = "will";
                var password = "password";
                client = new MatrixClient(homeserverUrl);
                var login = client.LoginWithPassword(username, password);
                File.WriteAllText("/tmp/mx_access", $"{login.AccessToken}${login.UserId}");
            }

            Console.WriteLine("Starting sync");
            client.StartSync();
            Console.WriteLine("Finished initial sync");
            foreach (var room in client.GetAllRooms()) Console.WriteLine($"Found room: {room.Id}");
        }
    }
}