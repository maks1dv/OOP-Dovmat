using System;

namespace Lab2
{
    public class Pet
    {
        private string _species;
        private string _nickname;
        private int _age;

        public string Species
        {
            get => _species;
            set => _species = string.IsNullOrWhiteSpace(value) ? "Unknown" : value;
        }

        public string Nickname
        {
            get => _nickname;
            set => _nickname = string.IsNullOrWhiteSpace(value) ? "Unnamed" : value;
        }

        public int Age
        {
            get => _age;
            set
            {
                if (value < 0)
                {
                    Console.WriteLine($"[Валідація] Помилка: Вік для '{Nickname}' не може бути від'ємним ({value}). Встановлено 0.");
                    _age = 0;
                }
                else
                {
                    _age = value;
                }
            }
        }

        public Pet(string species, string nickname, int age)
        {
            Species = species;
            Nickname = nickname;
            Age = age;
            Console.WriteLine($"[Конструктор 1] Створено об'єкт тварини: {Nickname} ({Species})");
        }

        public Pet() : this("Unknown", "Unnamed", 0)
        {
            Console.WriteLine("[Конструктор 2] Викликано конструктор за замовчуванням");
        }

        public void Speak()
        {
            string sound = Species.ToLower() switch
            {
                "dog" or "собака" or "пес" => "Гав-гав!",
                "cat" or "кіт" or "кішка" => "Мяу!",
                "cow" or "корова" => "Му-у-у!",
                _ => "Якийсь звук тварини..."
            };

            Console.WriteLine($" -> {Nickname} ({Species}, вік: {Age}) каже: {sound}");
        }

        ~Pet()
        {
            Console.WriteLine($"[Фіналізатор] Збирач сміття знищує об'єкт: {_nickname}");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== 1. Створення об'єктів (Виклики конструкторів) ===");
            
            Pet pet1 = new Pet("Собака", "Рекс", 3);
            Pet pet2 = new Pet();
            Pet pet3 = new Pet("Кіт", "Мурчик", -2);

            Console.WriteLine("\n=== 2. Демонстрація роботи методів ===");
            pet1.Speak();
            pet2.Speak();
            pet3.Speak();

            Console.WriteLine("\n=== 3. Завершення роботи Main та збір сміття (GC) ===");
            
            pet1 = null;
            pet2 = null;
            pet3 = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("Програму завершено.");
        }
    }
}