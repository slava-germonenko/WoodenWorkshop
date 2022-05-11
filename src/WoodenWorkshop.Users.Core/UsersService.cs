using Microsoft.EntityFrameworkCore;

using WoodenWorkshop.Common.Core.Exceptions;
using WoodenWorkshop.Users.Core.Errors;
using WoodenWorkshop.Users.Core.Models;

namespace WoodenWorkshop.Users.Core;

public class UsersService
{
    private readonly UsersContext _usersContext;

    public UsersService(UsersContext usersContext)
    {
        _usersContext = usersContext;
    }

    public async Task<User> GetUserAsync(int userId)
    {
        var user = await _usersContext.Users.FindAsync(userId);
        if (user is null)
        {
            throw new CoreLogicException(ErrorMessages.UserNotFound, ErrorCodes.UserNotFound);
        }

        return user;
    }

    public async Task<User> SaveUserAsync(User user)
    {
        var userEmailAddressIsInUse = await _usersContext.Users.AnyAsync(
            u => u.EmailAddress == user.EmailAddress && u.Id != user.Id
        );
        if (userEmailAddressIsInUse)
        {
            throw new CoreLogicException(ErrorMessages.UserEmailAddressIsInUse, ErrorCodes.UserEmailAddressIsInUse);
        }
        
        var userToSave = await _usersContext.Users.FindAsync(user.Id) ?? new User();
        userToSave.CopyDetails(user);
        _usersContext.Update(userToSave);
        await _usersContext.SaveChangesAsync();
        return userToSave;
    }

    public async Task RemoveUserAsync(int userId)
    {
        var user = await _usersContext.Users.FindAsync(userId);
        if (user is not null)
        {
            _usersContext.Users.Remove(user);
        }
    }
}