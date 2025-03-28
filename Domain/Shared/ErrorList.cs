using System.Runtime.InteropServices.JavaScript;

namespace Domain.Shared;

public static class ErrorList
{
    public static class Registration
    {
        public static readonly (string Code, string ExMessage) General = ("REG_001", "Ocorreu um erro imprevisto no registro no banco de dados.");
    }
    
    public static class Users
    {
        public static readonly (string Code, string ExMessage) General = ("USE_001", "Ocorreu um erro imprevisto no registro no banco de dados.");
    }
    
}