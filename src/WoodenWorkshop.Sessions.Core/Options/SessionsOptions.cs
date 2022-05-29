namespace WoodenWorkshop.Sessions.Core.Options;

public record SessionsOptions
{
    public int CleanupIntervalMinutes { get; set; }
    
    public int CleanupBatchSize { get; set; }
}