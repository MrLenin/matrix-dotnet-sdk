namespace Matrix.Client
{
    /**
     * Handles end to end key operations on behalf of the client.
     */
    public class Keys
    {
        private readonly MatrixApi _api;
        
        public Keys(MatrixApi api) => _api = api;
    }
}