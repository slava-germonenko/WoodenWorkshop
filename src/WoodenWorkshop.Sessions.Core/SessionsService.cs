using Microsoft.EntityFrameworkCore;

using WoodenWorkshop.Common.Core.Exceptions;
using WoodenWorkshop.Common.Core.Models;
using WoodenWorkshop.Common.EntityFramework.Extensions;
using WoodenWorkshop.Sessions.Core.Contracts;
using WoodenWorkshop.Sessions.Core.Dtos;
using WoodenWorkshop.Sessions.Core.Errors;
using WoodenWorkshop.Sessions.Core.Extensions;
using WoodenWorkshop.Sessions.Core.Models;

namespace WoodenWorkshop.Sessions.Core;

public class SessionsService
{
    private readonly SessionsContext _sessionsContext;
    
    private readonly ITokenGenerator _tokenGenerator;

    private readonly IUsersClient _usersClient;

    public SessionsService(
        SessionsContext sessionsContext,
        ITokenGenerator tokenGenerator, 
        IUsersClient usersClient
    )
    {
        _sessionsContext = sessionsContext;
        _tokenGenerator = tokenGenerator;
        _usersClient = usersClient;
    }

    
    public async Task<PagedResult<UserSession>> GetUserSessionsAsync(UserSessionsFilter filter)
    {
        return await _sessionsContext.UserSessions.AsNoTracking()
            .ApplyFilter(filter)
            .OrderByDescending(session => session.CreatedDate)
            .ToPagedResultAsync(filter);
    }

    public async Task<UserSession> StartSessionAsync(StartSessionDto sessionDto)
    {
        await EnsureUserCanStartSession(sessionDto.UserId);
        var startedSession = await _sessionsContext.UserSessions
            .AsNoTracking()
            .FirstOrDefaultAsync(
                session => session.UserId == sessionDto.UserId
                    && session.IpAddress == sessionDto.IpAddress
                    && session.DeviceName == sessionDto.DeviceName
            );

        var sessionStarted = startedSession is not null
                             && (startedSession.ExpireDate > DateTime.UtcNow || startedSession.ExpireDate is null);
        if (sessionStarted)
        {
            throw new CoreLogicException(ErrorMessages.SessionAlreadyStarted, ErrorCodes.SessionAlreadyStarted);
        }

        var newSession = startedSession ?? new();
        newSession.UserId = sessionDto.UserId;
        newSession.IpAddress = sessionDto.IpAddress;
        newSession.ExpireDate = sessionDto.ExpireDate;
        newSession.DeviceName = sessionDto.DeviceName;
        newSession.Description = sessionDto.Description;
        newSession.Token = _tokenGenerator.GenerateTokenUnique();

        _sessionsContext.UserSessions.Update(newSession);
        await _sessionsContext.SaveChangesAsync();

        return newSession;
    }

    public async Task<UserSession> RefreshSessionAsync(RefreshSessionDto sessionDto)
    {
        var sessionToRefresh = await _sessionsContext.UserSessions.FirstOrDefaultAsync(
            session => session.Token == sessionDto.Token 
        );
        if (sessionToRefresh is null)
        {
            throw new CoreLogicException(ErrorMessages.SessionNotFound, ErrorCodes.SessionNotFound);
        }

        await EnsureUserCanStartSession(sessionToRefresh.UserId);
        
        sessionToRefresh.ExpireDate = sessionDto.ExpireDate;
        sessionToRefresh.Token = _tokenGenerator.GenerateTokenUnique();

        _sessionsContext.UserSessions.Update(sessionToRefresh);
        await _sessionsContext.SaveChangesAsync();

        return sessionToRefresh;
    }

    public async Task TerminateSessionAsync(int sessionId)
    {
        var session = await _sessionsContext.UserSessions.FindAsync(sessionId);
        if (session is not null)
        {
            await EnsureUserCanStartSession(session.UserId);
            _sessionsContext.UserSessions.Remove(session);
            await _sessionsContext.SaveChangesAsync();
        }
    }

    public async Task TerminateSessionAsync(string token)
    {
        var session = await _sessionsContext.UserSessions.FirstOrDefaultAsync(s => s.Token == token);
        if (session is not null)
        {
            _sessionsContext.UserSessions.Remove(session);
            await _sessionsContext.SaveChangesAsync();
        }
    }

    private async Task EnsureUserCanStartSession(int userId)
    {
        var user = await _usersClient.GetUserAsync(userId);
        if (!user.Active)
        {
            throw new CoreLogicException(ErrorMessages.UserInactive, ErrorCodes.UserInactive);
        }
    }
}