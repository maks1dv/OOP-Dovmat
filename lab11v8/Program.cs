using System;
using System.Collections.Generic;

namespace lab11v8
{
    // ==========================================
    // 1. Абстрактний базовий клас (BackupService)
    // ==========================================
    public abstract class BackupService
    {
        // Властивості для зберігання стану обробки
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public double TotalSizeMb { get; protected set; }
        public DateTime ProcessedAt { get; private set; }
        public bool IsSuccessful { get; protected set; }

        protected BackupService(string sourcePath, string destinationPath)
        {
            SourcePath = sourcePath;
            DestinationPath = destinationPath;
        }

        // Template Method (Шаблонний метод) - задає жорсткий алгоритм із 5 кроків
        public void Backup()
        {
            Console.WriteLine($"\n==================================================");
            Console.WriteLine($"[СТАРТ БЕКАПУ] Джерело: {SourcePath}");
            Console.WriteLine($"==================================================");

            try
            {
                SelectFiles();
                Compress();

                if (ShouldEncrypt())
                {
                    Encrypt();
                }

                Upload();

                if (Verify())
                {
                    IsSuccessful = true;
                    ProcessedAt = DateTime.Now;
                    Console.WriteLine("[УСПІХ] Резервну копію успішно створено та перевірено!");
                }
                else
                {
                    IsSuccessful = false;
                    Console.WriteLine("[ПОМИЛКА] Верифікація цілісності бекапу провалилася.");
                }
            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                Console.WriteLine($"[КРИТИЧНА ПОМИЛКА] {ex.Message}");
            }

            Console.WriteLine($"--- Результат: {(IsSuccessful ? "Успішно" : "Провалено")} | Розмір: {TotalSizeMb:F2} MB | Час: {ProcessedAt:HH:mm:ss} ---");
        }

        // Абстрактні кроки (обов'язкові для реалізації в похідних класах)
        protected abstract void SelectFiles();
        protected abstract void Upload();

        // Віртуальні кроки (з базовою реалізацією, які можна перевизначити)
        protected virtual void Compress()
        {
            Console.WriteLine($"[Стиснення] Застосовано ZIP-стиснення. Розмір зменшено до {TotalSizeMb * 0.7:F2} MB.");
            TotalSizeMb *= 0.7;
        }

        protected virtual bool ShouldEncrypt()
        {
            return true; // За замовчуванням шифрування увімкнене
        }

        protected virtual void Encrypt()
        {
            Console.WriteLine("[Шифрування] Застосування AES-256 шифрування до архіву.");
        }

        protected virtual bool Verify()
        {
            Console.WriteLine("[Верифікація] Перевірка контрольної суми (SHA-256) створеного бекапу...");
            return true;
        }
    }

    // ==========================================
    // 2. Похідний клас 1: FullBackup
    // ==========================================
    public class FullBackup : BackupService
    {
        public bool IncludeSystemFiles { get; set; } = true;

        public FullBackup(string source, string dest) : base(source, dest) { }

        protected override void SelectFiles()
        {
            TotalSizeMb = 1024.0; // 1 GB
            Console.WriteLine($"[FullBackup] Повний відбір усіх файлів з '{SourcePath}' (Системні файли: {IncludeSystemFiles}).");
        }

        protected override void Upload()
        {
            Console.WriteLine($"[FullBackup] Збереження повного образу на локальний накопичувач: '{DestinationPath}'.");
        }
    }

    // ==========================================
    // 3. Похідний клас 2: IncrementalBackup
    // ==========================================
    public class IncrementalBackup : BackupService
    {
        public DateTime LastBackupTime { get; set; }

        public IncrementalBackup(string source, string dest, DateTime lastBackupTime) 
            : base(source, dest)
        {
            LastBackupTime = lastBackupTime;
        }

        protected override void SelectFiles()
        {
            TotalSizeMb = 120.5;
            Console.WriteLine($"[IncrementalBackup] Сканування файлів, змінених після {LastBackupTime:dd.MM.yyyy HH:mm}. Знайдено 120.5 MB.");
        }

        protected override void Upload()
        {
            Console.WriteLine($"[IncrementalBackup] Додавання лише змінених блоків до архіву '{DestinationPath}'.");
        }

        protected override bool ShouldEncrypt()
        {
            // Для швидких інкрементних копій шифрування вимкнено
            return false;
        }
    }

    // ==========================================
    // 4. Похідний клас 3: CloudBackup
    // ==========================================
    public class CloudBackup : BackupService
    {
        public string CloudProvider { get; set; }

        public CloudBackup(string source, string dest, string provider) 
            : base(source, dest)
        {
            CloudProvider = provider;
        }

        protected override void SelectFiles()
        {
            TotalSizeMb = 500.0;
            Console.WriteLine($"[CloudBackup] Вибір важливих користувацьких документів для хмарного сховища.");
        }

        protected override void Compress()
        {
            Console.WriteLine($"[CloudBackup] Максимальне LZMA-стиснення для заощадження трафіку. Розмір: {TotalSizeMb * 0.4:F2} MB.");
            TotalSizeMb *= 0.4;
        }

        protected override void Upload()
        {
            Console.WriteLine($"[CloudBackup] Завантаження по SSL/TLS в хмару '{CloudProvider}' (Контейнер: {DestinationPath}).");
        }

        protected override bool Verify()
        {
            base.Verify();
            Console.WriteLine($"[CloudBackup] Додаткова перевірка доступності об'єкта через REST API {CloudProvider}.");
            return true;
        }
    }

    // ==========================================
    // 5. Точка входу Main
    // ==========================================
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Створення списку об'єктів типу базового класу
            List<BackupService> backupTasks = new List<BackupService>
            {
                new FullBackup(@"C:\Data\Projects", @"D:\Backups\Full_System.bak") 
                { 
                    IncludeSystemFiles = true 
                },
                new IncrementalBackup(@"C:\Data\Projects", @"D:\Backups\Inc_Daily.bak", DateTime.Now.AddDays(-1)),
                new CloudBackup(@"C:\Users\Docs", "user-backups-bucket", "Google Cloud Storage")
            };

            // Виклик шаблонного методу Backup() для кожного об'єкта
            foreach (var task in backupTasks)
            {
                task.Backup();
            }
        }
    }
}