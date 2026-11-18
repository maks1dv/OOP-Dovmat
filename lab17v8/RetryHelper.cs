using System;
using System.Threading;

namespace lab17v8
{
    public static class RetryHelper
    {
        public static T ExecuteWithRetry<T>(
            Func<T> operation,
            int retryCount = 3,
            TimeSpan initialDelay = default,
            Func<Exception, bool>? shouldRetry = null)
        {
            if (initialDelay == default)
            {
                initialDelay = TimeSpan.FromMilliseconds(200);
            }

            int attempt = 0;
            while (true)
            {
                try
                {
                    attempt++;
                    Console.WriteLine($"[RetryHelper] Спроба #{attempt}...");
                    return operation();
                }
                catch (Exception ex)
                {
                    bool canRetry = shouldRetry == null || shouldRetry(ex);

                    if (!canRetry)
                    {
                        Console.WriteLine($"[RetryHelper] Виняток {ex.GetType().Name} ігнорується політикою shouldRetry. Повтор скасовано.");
                        throw;
                    }

                    if (attempt > retryCount)
                    {
                        Console.WriteLine($"[RetryHelper] Вичерпано всі {retryCount} повторних спроб. Остання помилка: {ex.Message}");
                        throw;
                    }

                    // Експоненційна затримка: initialDelay * 2^(attempt - 1)
                    double delayMs = initialDelay.TotalMilliseconds * Math.Pow(2, attempt - 1);
                    TimeSpan delay = TimeSpan.FromMilliseconds(delayMs);

                    Console.WriteLine($"[RetryHelper] Невдача ({ex.GetType().Name}): {ex.Message}");
                    Console.WriteLine($"[RetryHelper] Очікування {delay.TotalMilliseconds} мс перед спробою #{attempt + 1}...");
                    Thread.Sleep(delay);
                }
            }
        }

        public static void ExecuteWithRetry(
            Action operation,
            int retryCount = 3,
            TimeSpan initialDelay = default,
            Func<Exception, bool>? shouldRetry = null)
        {
            ExecuteWithRetry(() =>
            {
                operation();
                return true;
            }, retryCount, initialDelay, shouldRetry);
        }
    }
}
