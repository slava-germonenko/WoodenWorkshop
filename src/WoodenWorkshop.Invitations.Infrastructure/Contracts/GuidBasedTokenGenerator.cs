using WoodenWorkshop.Invitations.Core.Contracts;

namespace WoodenWorkshop.Invitations.Infrastructure.Contracts;

public class GuidBasedTokenGenerator : ITokenGenerator
{
    public string GenerateUniqueToken() => Guid.NewGuid()
        .ToString()
        .ToLower()
        .Replace("-", null);
}