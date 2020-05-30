using System;

using Matrix.Api.Versions;
using Matrix.Structures;

namespace Matrix.Abstractions
{
    public class ProfileApi
    {
        private readonly MatrixApi _matrixApi;

        public ProfileApi(MatrixApi matrixApi) =>
            _matrixApi = matrixApi ?? throw new ArgumentNullException(nameof(matrixApi));

        [MatrixSpec(ClientServerApiVersion.R001, "get-matrix-client-r0-profile-userid")]
        public virtual MatrixProfile GetProfile(string userId)
        {
            _matrixApi.ThrowIfNotSupported();

            var apiPath = new Uri("/_matrix/client/r0/profile/" + userId, UriKind.Relative);
            //var error = _matrixApi.Backend.HandleGet(apiPath, true, out var response);

            //return error.IsOk ? response.ToObject<MatrixProfile>() : null;
            throw new NotImplementedException();
        }

        [MatrixSpec(ClientServerApiVersion.R001, "get-matrix-client-r0-profile-displayname")]
        public void SetDisplayName(string userId, string displayName)
        {
            _matrixApi.ThrowIfNotSupported();

            //var request = new JObject
            //{
            //    {"displayname", JToken.FromObject(displayName)}
            //};

            //var apiPath = new Uri($"/_matrix/client/r0/profile/{Uri.EscapeUriString(userId)}/displayname",
            //    UriKind.Relative);
            //var error = _matrixApi.Backend.HandlePut(apiPath, true, request, out _);

            //if (!error.IsOk) throw new MatrixException(error.ToString());
        }

        [MatrixSpec(ClientServerApiVersion.R001, "get-matrix-client-r0-profile-userid-displayname")]
        public void SetAvatar(string userId, Uri avatarUrl)
        {
            _matrixApi.ThrowIfNotSupported();

            //var request = new JObject {{"avatar_url", JToken.FromObject(avatarUrl)}};
            //var apiPath = new Uri($"/_matrix/client/r0/profile/{Uri.EscapeUriString(userId)}/avatar_url",
            //    UriKind.Relative);
            //var error = _matrixApi.Backend.HandlePut(apiPath, true, request, out _);

            //if (!error.IsOk) throw new MatrixException(error.ToString());
        }
    }
}