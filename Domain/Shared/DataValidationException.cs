namespace Domain.Shared;
public class DataValidationException : Exception
{
    public string Code;
    public string ExMessage;
    public string? Details;
    public List<Field>? Fields = new();

    private DataValidationException(string errorCode, string message, string details, List<Field> fields)
    {
        Code = errorCode;
        ExMessage = message;
        Details = details;
        Fields.AddRange(fields);
    }

    public static void Throw(string errorCode, string message, string details, List<Field> fields)
    {
        throw new DataValidationException(errorCode, message, details, fields);
    }
}
