namespace Domain.Shared
{
    public class ServerException : Exception
    {
        public string Code;
        public string ExMessage;

        public ServerException(string errorCode, string message)
        {
            Code = errorCode;
            ExMessage = message;
        }

        public static void Throw(string errorCode, string message)
        {
            throw new ServerException(errorCode, message);
        }
    }
}
