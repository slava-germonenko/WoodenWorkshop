using WoodenWorkshop.Invitations.Core.Dtos;

namespace WoodenWorkshop.Invitations.Core.Contracts;

public interface IUsersClient
{
    public Task<UserDto> CreateNewUserAsync(UserDto userDto);

    public Task<UserDto?> GetUserByEmailAddressAsync(string emailAddress);
}