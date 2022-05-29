namespace WoodenWorkshop.Passwords.Infrastructure.Options;

public record SecurityOptions
{
    public int PasswordSaltLength { get; set; }
    
    public int PasswordHashBytesLength { get; set; }
    
    public int PasswordHashIterationCount { get; set; }
}