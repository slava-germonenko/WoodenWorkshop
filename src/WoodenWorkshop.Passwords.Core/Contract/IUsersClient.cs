using WoodenWorkshop.Passwords.Core.Dtos;

namespace WoodenWorkshop.Passwords.Core.Contract;

public interface IUsersClient
{
    public Task<UserDto> GetUserAsync(int userId);

    public Task<UserDto> UpdateUserAsync(UserDto user);
}