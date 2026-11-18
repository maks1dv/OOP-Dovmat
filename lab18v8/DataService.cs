using System.Collections.Generic;

namespace lab18v8
{
    public class DataService
    {
        private readonly string _connectionString;
        private bool _isConnected;
        private readonly List<string> _storage = new List<string>
        {
            "ID: 1 | Студент: Максим Довмат | Група: ІПЗ-21",
            "ID: 2 | Студент: Олена Коваль | Група: ІПЗ-21",
            "ID: 3 | Студент: Іван Петренко | Група: ІПЗ-22"
        };

        public DataService(string connectionString, bool isConnected = false)
        {
            _connectionString = connectionString;
            _isConnected = isConnected;
        }

        public void Connect() => _isConnected = true;
        public void Disconnect() => _isConnected = false;

        // Викидає власний виняток при відсутності підключення до БД
        public List<string> GetData(string query)
        {
            if (!_isConnected || string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new DatabaseConnectionException(
                    _connectionString,
                    $"Помилка виконання запиту '{query}': з'єднання з БД не встановлено.");
            }

            return new List<string>(_storage);
        }

        // Повертає Result<T> для контрольованої обробки невалідних вхідних даних
        public Result<bool> SaveData(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return Result<bool>.Failure("Дані для збереження не можуть бути порожніми.");
            }

            if (data.Length < 5)
            {
                return Result<bool>.Failure($"Рядок '{data}' занадто короткий (мінімум 5 символів).");
            }

            if (!_isConnected)
            {
                return Result<bool>.Failure(
                    new DatabaseConnectionException(_connectionString, "Неможливо зберегти запис: немає з'єднання з БД."));
            }

            _storage.Add(data);
            return Result<bool>.Success(true);
        }
    }
}
