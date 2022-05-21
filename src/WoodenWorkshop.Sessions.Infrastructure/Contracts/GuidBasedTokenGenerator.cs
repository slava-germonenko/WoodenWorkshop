using WoodenWorkshop.Sessions.Core.Contracts;

namespace WoodenWorkshop.Sessions.Infrastructure.Contracts;

public class GuidBasedTokenGenerator : ITokenGenerator
{
    public string GenerateTokenUnique() => Guid.NewGuid()
        .ToString()
        .ToLower()
        .Replace("-", null);
}