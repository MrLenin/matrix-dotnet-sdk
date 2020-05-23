using System;
using Matrix.Structures;

namespace Matrix.Client
{
    /// <summary>
    /// A representation of a Matrix User.
    /// Contains basic profile information.
    /// </summary>
    public class MatrixUser
    {
        /// <summary>
        /// This constructor is intended for the API only.
        /// Create a new user from a profile & userid.
        /// </summary>
        /// <param name="profile">Profile.</param>
        /// <param name="userId">Userid.</param>
        public MatrixUser(MatrixProfile profile, string userId)
        {
            _profile = profile;
            UserId = userId;
        }

        private readonly MatrixProfile _profile;

        public Uri AvatarUrl => _profile.AvatarUrl;
        public string DisplayName => _profile.Displayname;
        public string UserId { get; }
    }
}