using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<EMatrixSpecApiVersion> SupportedVersions()
        {
            return _versionsList.ConvertAll(MatrixSpecAttribute.GetVersionForString);
        }
    }
}