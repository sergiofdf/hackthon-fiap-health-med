namespace Domain.Shared
{
    public class NotFoundException : Exception
    {
        public string Code;
        public string ExMessage;

        private NotFoundException(string errorCode, string message)
        {
            Code = errorCode;
            ExMessage = message;
        }

        public static void Throw(string errorCode, string message)
        {
            throw new NotFoundException(errorCode, message);
        }
    }
}
