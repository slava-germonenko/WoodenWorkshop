namespace WoodenWorkshop.Invitations.Core.Dtos;

public class UserDto
{
    public int Id { get; set; }
    
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;
}