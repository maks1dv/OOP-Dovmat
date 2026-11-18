using System;
using System.Collections.Generic;

namespace lab18v8
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== ЛАБОРАТОРНА РОБОТА №18: ВЛАСНІ ВИНЯТКИ ТА ПАТЕРН RESULT (ВАРІАНТ 8) ===\n");

            string connStr = "Server=db.college.edu.ua;Database=StudentsDB;User=dovmat;";
            var dataService = new DataService(connStr, isConnected: false);

            // -------------------------------------------------------------
            // 1. Виклик GetData без підключення -> обробка DatabaseConnectionException через try-catch
            // -------------------------------------------------------------
            Console.WriteLine("--- 1. ВЛАСНИЙ ВИНЯТОК: Спроба отримати дані без з'єднання з БД ---");
            try
            {
                List<string> records = dataService.GetData("SELECT * FROM Students");
                Console.WriteLine($"Отримано записів: {records.Count}");
            }
            catch (DatabaseConnectionException ex)
            {
                Console.WriteLine($"[Перехоплено DatabaseConnectionException]: {ex.Message}");
                Console.WriteLine($"[Рядок підключення]: {ex.ConnectionString}");
            }

            // -------------------------------------------------------------
            // 2. Підключення до БД та успішне отримання даних
            // -------------------------------------------------------------
            Console.WriteLine("\n--- 2. ВСТАНОВЛЕННЯ З'ЄДНАННЯ ТА ЧИТАННЯ ДАНИХ ---");
            dataService.Connect();
            try
            {
                List<string> records = dataService.GetData("SELECT * FROM Students");
                Console.WriteLine($"Успішно завантажено записів ({records.Count}):");
                foreach (var item in records)
                {
                    Console.WriteLine($" - {item}");
                }
            }
            catch (DatabaseConnectionException ex)
            {
                Console.WriteLine($"[Помилка БД]: {ex.Message}");
            }

            // -------------------------------------------------------------
            // 3. Патерн Result<T>: перевірка SaveData з невалідними та валідними даними
            // -------------------------------------------------------------
            Console.WriteLine("\n--- 3. ПАТЕРН RESULT<T>: Збереження даних (валідація без винятків) ---");

            var invalidEmptyResult = dataService.SaveData("   ");
            if (!invalidEmptyResult.IsSuccess)
            {
                Console.WriteLine($"[SaveData(\"\") -> Failure]: {invalidEmptyResult.Error}");
            }

            var invalidShortResult = dataService.SaveData("abc");
            if (!invalidShortResult.IsSuccess)
            {
                Console.WriteLine($"[SaveData(\"abc\") -> Failure]: {invalidShortResult.Error}");
            }

            string newStudent = "ID: 4 | Студент: Анна Сидоренко | Група: ІПЗ-22";
            var validResult = dataService.SaveData(newStudent);
            if (validResult.IsSuccess)
            {
                Console.WriteLine($"[SaveData -> Success ({validResult.Value})]: Додано запис -> {newStudent}");
            }
            else
            {
                Console.WriteLine($"[SaveData -> Failure]: {validResult.Error}");
            }

            Console.WriteLine("\n=== ВИКОНАННЯ ЛАБОРАТОРНОЇ РОБОТИ №18 ЗАВЕРШЕНО ===");
        }
    }
}
