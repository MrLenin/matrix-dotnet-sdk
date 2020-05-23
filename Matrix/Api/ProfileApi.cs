using System;
using Matrix.Structures;
using Newtonsoft.Json.Linq;

namespace Matrix
{
    public partial class MatrixApi
    {
        [MatrixSpec(EMatrixSpecApiVersion.R001, EMatrixSpecApi.ClientServer, "get-matrix-client-r0-profile-userid")]
        public virtual MatrixProfile ClientProfile(string userId)
        {
            ThrowIfNotSupported();

            var apiPath = new Uri("/_matrix/client/r0/profile/" + userId, UriKind.Relative);
            var error = _matrixApiBackend.HandleGet(apiPath, true, out var response);

            return error.IsOk ? response.ToObject<MatrixProfile>() : null;
        }

        [MatrixSpec(EMatrixSpecApiVersion.R001, EMatrixSpecApi.ClientServer,
            "get-matrix-client-r0-profile-displayname")]
        public void ClientSetDisplayName(string userId, string displayName)
        {
            ThrowIfNotSupported();

            var request = new JObject
            {
                {"displayname", JToken.FromObject(displayName)}
            };

            var apiPath = new Uri($"/_matrix/client/r0/profile/{Uri.EscapeUriString(userId)}/displayname",
                UriKind.Relative);
            var error = _matrixApiBackend.HandlePut(apiPath, true, request, out _);

            if (!error.IsOk) throw new MatrixException(error.ToString());
        }

        [MatrixSpec(EMatrixSpecApiVersion.R001, EMatrixSpecApi.ClientServer,
            "get-matrix-client-r0-profile-userid-displayname")]
        public void ClientSetAvatar(string userId, Uri avatarUrl)
        {
            ThrowIfNotSupported();

            var request = new JObject {{"avatar_url", JToken.FromObject(avatarUrl)}};
            var apiPath = new Uri($"/_matrix/client/r0/profile/{Uri.EscapeUriString(userId)}/avatar_url",
                UriKind.Relative);
            var error = _matrixApiBackend.HandlePut(apiPath, true, request, out _);

            if (!error.IsOk) throw new MatrixException(error.ToString());
        }
    }
}