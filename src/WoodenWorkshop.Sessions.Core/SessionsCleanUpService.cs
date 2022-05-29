using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using WoodenWorkshop.Sessions.Core.Options;

namespace WoodenWorkshop.Sessions.Core;

public class SessionsCleanUpService
{
    private readonly SessionsContext _sessionsContext;

    private readonly IOptionsSnapshot<SessionsOptions> _sessionsOptions;

    private int CleanupBatchSize => _sessionsOptions.Value.CleanupBatchSize;

    public SessionsCleanUpService(SessionsContext sessionsContext, IOptionsSnapshot<SessionsOptions> sessionsOptions)
    {
        _sessionsContext = sessionsContext;
        _sessionsOptions = sessionsOptions;
    }

    public async Task CleanupExpiredSessionsAsync()
    {
        var tokensToCleanUp = await _sessionsContext.UserSessions.AsNoTracking()
            .Where(session => session.ExpireDate != null && session.ExpireDate < DateTime.UtcNow)
            .Take(CleanupBatchSize)
            .ToListAsync();
        
        _sessionsContext.UserSessions.RemoveRange(tokensToCleanUp);
        await _sessionsContext.SaveChangesAsync();
    }
}