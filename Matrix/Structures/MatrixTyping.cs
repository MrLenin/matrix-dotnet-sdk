using System.Collections.Generic;

namespace Matrix.Structures
{
    /// <summary>
    /// Following https://matrix.org/docs/spec/r0.0.1/client_server.html#m-typing
    /// </summary>
    public class MatrixMTyping : MatrixEventContent
    {
        public IEnumerable<string> UserIds { get; set; }
    }
}