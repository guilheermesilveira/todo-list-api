namespace TodoList.Application.Configurations;

public class JwtSettings
{
    public string SecretKey { get; set; } = null!;
    public int HoursToExpire { get; set; }
}