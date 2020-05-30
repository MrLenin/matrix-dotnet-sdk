using System;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Matrix.Client;
using Matrix.Json;
using Matrix.Structures;

namespace Matrix.AppService
{
    internal struct EventBatch
    {
        public MatrixEvent[] Events;
    }

    public delegate void AliasRequestDelegate(string alias, out bool roomExists);

    public delegate void UserRequestDelegate(string userid, out bool userExists);

    public delegate void EventDelegate(MatrixEvent ev);

    public class MatrixAppservice : IDisposable
    {
        private readonly Uri DefaultUri = new Uri("http://localhost");
        private const int DefaultMaxRequests = 64;

        public Uri AppserviceUrl { get; }
        public Uri HomeserverUrl { get; }
        public int MaximumRequests { get; }
        public string Domain { get; }
        public ServiceRegistration Registration { get; }

        public event AliasRequestDelegate OnAliasRequest;
        public event EventDelegate OnEvent;
        public event UserRequestDelegate OnUserRequest;

        private Semaphore _acceptSemaphore;
        private HttpListener _listener;
        private readonly Regex _urlMatcher;
        private readonly MatrixApi _api;
        private readonly string _botUserId;

        public MatrixAppservice(ServiceRegistration registration, string domain,
            Uri url, int maxRequests = DefaultMaxRequests)
        {
            HomeserverUrl = url ?? DefaultUri;
            Domain = domain ?? throw new ArgumentNullException(nameof(domain));
            MaximumRequests = maxRequests;
            Registration = registration ?? throw new ArgumentNullException(nameof(registration));
            AppserviceUrl = registration.Url;
            _botUserId = "@" + registration.Localpart + ":" + Domain;
            _urlMatcher =
                new Regex("\\/(rooms|transactions|users)\\/(.+)\\?access_token=(.+)",
                    RegexOptions.Compiled | RegexOptions.ECMAScript);

            _api = new MatrixApi(url, registration.AppserviceToken, "");
        }

        public void Run()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(AppserviceUrl + "/rooms/");
            _listener.Prefixes.Add(AppserviceUrl + "/transactions/");
            _listener.Prefixes.Add(AppserviceUrl + "/users/");
            _listener.Start();

            _acceptSemaphore = new Semaphore(MaximumRequests, MaximumRequests);

            while (_listener.IsListening)
            {
                _acceptSemaphore.WaitOne();
                _ = _listener.GetContextAsync().ContinueWith(HttpContextHandler, TaskScheduler.Default);
            }
        }

        public MatrixClient GetClientAsUser(string user = null)
        {
            if (user != null)
            {
                if (user.EndsWith(":" + Domain, StringComparison.InvariantCulture))
                    user = user.Substring(0, user.LastIndexOf(':'));

                if (user.StartsWith("@", StringComparison.InvariantCulture))
                    user = user.Substring(1);

                CheckAndPerformRegistration(user);

                user = "@" + user;
                user = user + ":" + Domain;
            }
            else
            {
                user = _botUserId;
            }

            return new MatrixClient(HomeserverUrl, Registration.AppserviceToken, user);
        }

        private void CheckAndPerformRegistration(string user)
        {
            if (_api.Profile.GetProfile("@" + user + ":" + Domain) == null)
                _api.RegisterUserAsAppservice(user);
        }

        private async void HttpContextHandler(Task<HttpListenerContext> task)
        {
            await task.ConfigureAwait(false);

            var context = task.Result;
            var match = _urlMatcher.Match(context.Request.RawUrl);

            if (match.Groups.Count != 4)
            {
                context.Response.StatusCode = (int) HttpStatusCode.BadRequest; //Invalid response
                context.Response.Close();
                _acceptSemaphore.Release();
                return;
            }

            if (match.Groups[3].Value != Registration.HomeserverToken)
            {
                context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                context.Response.Close();
                _acceptSemaphore.Release();
            }

            var type = match.Groups[1].Value;
            context.Response.StatusCode = (int) HttpStatusCode.OK;

            //Check methods
            switch (type)
            {
                case "users":
                case "rooms":
                    if (context.Request.HttpMethod != "GET")
                        context.Response.StatusCode = (int) HttpStatusCode.MethodNotAllowed;
                    break;

                case "transactions":
                    if (context.Request.HttpMethod != "PUT")
                        context.Response.StatusCode = (int) HttpStatusCode.MethodNotAllowed;
                    break;
            }

            var exists = false;

            if (context.Response.StatusCode == (int) HttpStatusCode.OK)
                switch (type)
                {
                    case "rooms":
                        context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                        var alias = Uri.UnescapeDataString(match.Groups[2].Value);
                        OnAliasRequest?.Invoke(alias, out exists);
                        break;

                    case "transactions":
                        var data = new byte[context.Request.ContentLength64];
                        context.Request.InputStream.Read(data, 0, data.Length);
                        var batch = System.Text.Json.JsonSerializer.Deserialize<EventBatch>(
                            Encoding.UTF8.GetString(data));
                        foreach (var ev in batch.Events)
                            OnEvent?.Invoke(ev);

                        break;

                    case "users":
                        var user = Uri.UnescapeDataString(match.Groups[2].Value);
                        OnUserRequest?.Invoke(user, out exists);
                        context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                        break;
                }

            if (exists)
                context.Response.StatusCode = 200;

            context.Response.OutputStream.Write(new byte[] {123, 125}, 0, 2);
            context.Response.Close();
            _acceptSemaphore.Release();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            _listener?.Close();
            _acceptSemaphore?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}