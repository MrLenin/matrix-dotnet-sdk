using System;
using System.Collections.Generic;
using System.Linq;

using Matrix.Api.Versions;

namespace Matrix.Api.ClientServer
{
    public class VersionsContext
    {
        public IDictionary<string, bool> UnstableFeatures { get; set; }
        public IEnumerable<ClientServerVersion> Versions { get; set; }
    }
}