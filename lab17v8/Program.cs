using System;
using System.IO;
using System.Net.Http;

namespace lab17v8
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== ЛАБОРАТОРНА РОБОТА №17: ОБРОБКА IO/МЕРЕЖЕВИХ ПОМИЛОК ТА ПАТЕРН RETRY (ВАРІАНТ 8) ===\n");

            // Політика повторів: повторювати лише для IOException та HttpRequestException
            Func<Exception, bool> retryPolicy = ex => ex is IOException || ex is HttpRequestException;

            var fileProcessor = new FileProcessor();
            var networkClient = new NetworkClient();
            string userJson = "{\"id\": 8, \"name\": \"Максим Довмат\", \"role\": \"Student\"}";

            // -------------------------------------------------------------
            // Сценарій 1: Збереження даних у файл (4 помилки IOException -> успіх на 5-й спробі)
            // -------------------------------------------------------------
            Console.WriteLine("--- 1. СЦЕНАРІЙ FILEPROCESSOR: SaveUserData (очікується 4 невдачі, 5-та успішна) ---");
            try
            {
                RetryHelper.ExecuteWithRetry(
                    operation: () => fileProcessor.SaveUserData("users_backup.json", userJson),
                    retryCount: 4,
                    initialDelay: TimeSpan.FromMilliseconds(200),
                    shouldRetry: retryPolicy
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Критична помилка файлу]: {ex.Message}");
            }

            // -------------------------------------------------------------
            // Сценарій 2: Відправка даних на сервер (2 помилки HttpRequestException -> успіх на 3-й спробі)
            // -------------------------------------------------------------
            Console.WriteLine("\n--- 2. СЦЕНАРІЙ NETWORKCLIENT: PostUserData (очікується 2 невдачі, 3-тя успішна) ---");
            try
            {
                bool isSent = RetryHelper.ExecuteWithRetry(
                    operation: () => networkClient.PostUserData("https://api.college.edu.ua/users", userJson),
                    retryCount: 3,
                    initialDelay: TimeSpan.FromMilliseconds(200),
                    shouldRetry: retryPolicy
                );
                Console.WriteLine($"Результат мережевого запиту: {isSent}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Критична помилка мережі]: {ex.Message}");
            }

            // -------------------------------------------------------------
            // Сценарій 3: Перевірка вибіркової обробки через shouldRetry (неповторюваний виняток)
            // -------------------------------------------------------------
            Console.WriteLine("\n--- 3. ДЕМОНСТРАЦІЯ SHOULDRETRY: Виняток ArgumentException (не повинен повторюватися) ---");
            try
            {
                RetryHelper.ExecuteWithRetry<bool>(
                    operation: () => throw new ArgumentException("Невалідний JSON-формат даних користувача."),
                    retryCount: 3,
                    initialDelay: TimeSpan.FromMilliseconds(200),
                    shouldRetry: retryPolicy
                );
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"[Перехоплено у Main без повторів]: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("\n[Finally] Блок завершення роботи виконано. Ресурси звільнено.");
            }

            Console.WriteLine("=== ВИКОНАННЯ ЛАБОРАТОРНОЇ РОБОТИ №17 ЗАВЕРШЕНО ===");
        }
    }
}
