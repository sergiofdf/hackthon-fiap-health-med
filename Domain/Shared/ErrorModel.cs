namespace Domain.Shared;
public class ErrorModel
{
    public string Code { get; set; }
    public string ExMessage { get; set; }

    public ErrorModel(string code, string message)
    {
        Code = code;
        ExMessage = message;
    }
}