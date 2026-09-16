using System;

namespace Lab1App
{
    public class Animal
    {
        private string _species;
        private string _nickname;
        private int _age;

        public string Species
        {
            get { return _species; }
            set { _species = value; }
        }

        public string Nickname
        {
            get { return _nickname; }
            set { _nickname = value; }
        }

        public int Age
        {
            get { return _age; }
            set
            {
                if (value >= 0)
                {
                    _age = value;
                }
                else
                {
                    Console.WriteLine("Помилка: вік не може бути від'ємним!");
                }
            }
        }

        public Animal(string species, string nickname, int age)
        {
            _species = species;
            _nickname = nickname;
            Age = age;
            Console.WriteLine($"[Конструктор]: Створено об'єкт {Species} на ім'я '{Nickname}'");
        }

        ~Animal()
        {
            Console.WriteLine($"[Деструктор]: Об'єкт '{Nickname}' вилучено з пам'яті");
        }

        public void Speak()
        {
            Console.WriteLine($"{Nickname} ({Species}, вік: {Age}) подає голос!");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Animal dog = new Animal("Собака", "Рекс", 3);
            Animal cat = new Animal("Кіт", "Барсик", 2);
            Animal parrot = new Animal("Папуга", "Кеша", 5);

            dog.Speak();
            cat.Speak();
            parrot.Speak();

            cat.Age = 3;
            Console.WriteLine($"Новий вік {cat.Nickname}: {cat.Age} р.");
            cat.Age = -1;
        }
    }
}