namespace Domain.Shared
{
    public class ForbiddenException : Exception
    {
        public string Code;
        public string ExMessage;

        private ForbiddenException(string errorCode, string message)
        {
            Code = errorCode;
            ExMessage = message;
        }

        public static void Throw(string errorCode, string message)
        {
            throw new ForbiddenException(errorCode, message);
        }
    }
}
