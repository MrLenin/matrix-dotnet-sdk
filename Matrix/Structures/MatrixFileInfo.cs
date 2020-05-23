namespace Matrix.Structures
{
    public class MatrixFileInfo
    {
        public string MimeType { get; set; }
        public int Size { get; set; }
    }

    public class MatrixImageInfo : MatrixFileInfo
    {
        public int Height { get; set; }
        public int Width { get; set; }
    }
}