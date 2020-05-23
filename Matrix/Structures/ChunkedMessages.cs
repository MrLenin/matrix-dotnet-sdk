using System.Collections.Generic;

namespace Matrix.Structures
{
    public class ChunkedMessages
    {
        public string Start { get; set; }
        public string End { get; set; }
        public IEnumerable<MatrixEvent> Chunk { get; set; }
    }
}