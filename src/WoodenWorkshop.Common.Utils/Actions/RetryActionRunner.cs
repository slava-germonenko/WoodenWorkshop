namespace WoodenWorkshop.Common.Utils.Actions;

public class RetryActionRunner
{
    private readonly TimeSpan _retryInterval;

    private readonly int _maxRetryAttempts;

    public RetryActionRunner(TimeSpan retryInterval, int maxRetryAttempts)
    {
        _retryInterval = retryInterval;
        _maxRetryAttempts = maxRetryAttempts;
    }

    public async Task RunWithRetryAsync(Action actionToRetry)
    {
        for (var attemptsUsed = 0; attemptsUsed < _maxRetryAttempts; attemptsUsed++)
        {
            try
            {
                actionToRetry.Invoke();
                return;
            }
            catch (Exception)
            {
                if (attemptsUsed == _maxRetryAttempts - 1)
                {
                    throw;
                }

                await Task.Delay(_retryInterval);
            }
        }
    }

    public async Task<T> RunWithRetryAsync<T>(Func<T> actionToRetry)
    {
        for (var attemptsUsed = 0; attemptsUsed < _maxRetryAttempts; attemptsUsed++)
        {
            try
            {
                return actionToRetry.Invoke();
            }
            catch (Exception)
            {
                if (attemptsUsed == _maxRetryAttempts - 1)
                {
                    throw;
                }

                await Task.Delay(_retryInterval);
            }
        }

        throw new Exception("Unreachable code.");
    }
}