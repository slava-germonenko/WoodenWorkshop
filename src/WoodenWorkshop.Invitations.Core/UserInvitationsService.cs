using Microsoft.EntityFrameworkCore;

using WoodenWorkshop.Common.Core.Exceptions;
using WoodenWorkshop.Invitations.Core.Contracts;
using WoodenWorkshop.Invitations.Core.Dtos;
using WoodenWorkshop.Invitations.Core.Errors;
using WoodenWorkshop.Invitations.Core.Models;
using WoodenWorkshop.Invitations.Core.Models.Enums;

namespace WoodenWorkshop.Invitations.Core;

public class UserInvitationsService
{
    private const string UserInvitationEmailSubject = "Приглашение в мастерскую Germonenko.by";
    
    private readonly IUserInvitationEmailCompiler _userInvitationEmailCompiler;

    private readonly IMailingClient _mailingClient;

    private readonly InvitationsContext _context;

    private readonly ITokenGenerator _tokenGenerator;
    
    private readonly IPasswordsClient _passwordsClient;
    
    private readonly IUsersClient _usersClient;

    public UserInvitationsService(
        IUserInvitationEmailCompiler userInvitationEmailCompiler,
        IMailingClient mailingClient,
        InvitationsContext context,
        ITokenGenerator tokenGenerator,
        IPasswordsClient passwordsClient,
        IUsersClient usersClient
    )
    {
        _userInvitationEmailCompiler = userInvitationEmailCompiler;
        _mailingClient = mailingClient;
        _context = context;
        _tokenGenerator = tokenGenerator;
        _passwordsClient = passwordsClient;
        _usersClient = usersClient;
    }

    public async Task<Invitation> InviteUserAsync(InviteUserDto inviteUserDto)
    {
        await EnsureUserEmailAddressIsNotInUse(inviteUserDto.EmailAddress);
        await DeactivateUserInvitationsAsync(inviteUserDto.EmailAddress);
        
        var newInvitation = inviteUserDto.ToInvitation();
        newInvitation.UniqueToken = _tokenGenerator.GenerateUniqueToken();
        _context.Invitations.Add(newInvitation);
        await _context.SaveChangesAsync();

        var invitationMail = new InvitationMailDto
        {
            Subject = UserInvitationEmailSubject,
            EmailAddress = inviteUserDto.EmailAddress,
            Body = await _userInvitationEmailCompiler.CompileUserInvitationAsync(newInvitation.UniqueToken),
        };
        await _mailingClient.QueueInvitationEmailAsync(invitationMail);
        return newInvitation;
    }

    public async Task AcceptUserInvitationAsync(AcceptUserInvitationData acceptInvitationData)
    {
        var invitation = await _context.Invitations.FirstOrDefaultAsync(
            i => i.UniqueToken == acceptInvitationData.Token && i.Type == InvitationTypes.NewUser
        );
        if (invitation is null)
        {
            throw new CoreLogicException(ErrorMessages.InvitationNotFound, ErrorCodes.InvitationNotFound);
        }

        if (!invitation.Active || invitation.ExpireDate < DateTime.UtcNow)
        {
            throw new CoreLogicException(ErrorMessages.InvitationInactive, ErrorCodes.InvitationInactive);
        }

        await EnsureUserEmailAddressIsNotInUse(invitation.EmailAddress);

        invitation.MarkAsAccepted(true);
        _context.Invitations.Update(invitation);
        await _context.SaveChangesAsync();
        
        var user = new UserDto
        {
            FirstName = acceptInvitationData.FirstName,
            LastName = acceptInvitationData.LastName,
            EmailAddress = invitation.EmailAddress,
            PasswordHash = string.Empty,
        };

        user = await _usersClient.CreateNewUserAsync(user);
        await _passwordsClient.SetUserPasswordAsync(user.Id, acceptInvitationData.Password);
    }

    public async Task DeclineUserInvitationAsync(string token)
    {
        var invitation = await _context.Invitations.FirstOrDefaultAsync(
            i => i.UniqueToken == token && i.Type == InvitationTypes.NewUser
        );
        if (invitation is null)
        {
            throw new CoreLogicException(ErrorMessages.InvitationNotFound, ErrorCodes.InvitationNotFound);
        }
        
        invitation.MarkAsAccepted(false);
        _context.Invitations.Update(invitation);
        await _context.SaveChangesAsync();
    }

    private async Task DeactivateUserInvitationsAsync(string emailAddress)
    {
        var invitationsToExpire = await _context.Invitations.Where(
            invitation => invitation.EmailAddress == emailAddress 
                          && invitation.Type == InvitationTypes.NewUser 
                          && invitation.Active
        ).ToListAsync();

        invitationsToExpire.ForEach(invitation => invitation.Active = false);
        _context.Invitations.UpdateRange(invitationsToExpire);
    }

    private async Task EnsureUserEmailAddressIsNotInUse(string emailAddress)
    {
        var user = await _usersClient.GetUserByEmailAddressAsync(emailAddress);
        if (user is not null)
        {
            throw new CoreLogicException(ErrorMessages.UserEmailAddressIsInUse, ErrorCodes.UserEmailAddressIsInUse);
        }
    }
}