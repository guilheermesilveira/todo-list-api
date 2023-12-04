namespace ToDo.Application.Configurations;

public class AppSettings
{
    public string Secret { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string ValidOn { get; set; } = null!;
}