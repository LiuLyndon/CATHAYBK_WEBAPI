namespace CATHAYBK_Service.Base
{
    public class CustomServiceException : Exception
    {
        public CustomServiceException(string message) : base(message) { }

        public CustomServiceException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
