namespace TodoList.Application.Configurations;

public class AppSettings
{
    public string Secret { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int TokenExpiration { get; set; }
}