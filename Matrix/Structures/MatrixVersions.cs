using System.Collections.Generic;
using System.Linq;

using Matrix.Api.Versions;

namespace Matrix.Structures
{
    public class MatrixVersions
    {
        private List<string> _versionsList;

        public Dictionary<string, bool> UnstableFeatures { get; set; }

        public IEnumerable<string> Versions
        {
            get => _versionsList;
            set => _versionsList = value as List<string>;
        }

        public IEnumerable<ClientServerApiVersion> SupportedVersions()
        {
            return _versionsList.ConvertAll(ClientServerApiVersionExtensions.FromJsonString);
        }
    }
}