using System;
using System.Collections.Generic;

namespace Lab13Variant8
{
    // ===================================================
    // 1. КЛАС ФУНКЦІОНАЛЬНИХ УТИЛІТ З GENERIC-МЕТОДАМИ
    // ===================================================
    public static class FunctionalUtils
    {
        // 1. Map: перетворює елементи List<T> у List<TResult>
        public static List<TResult> Map<T, TResult>(List<T> list, Func<T, TResult> mapper)
        {
            var result = new List<TResult>();
            foreach (var item in list)
            {
                result.Add(mapper(item));
            }
            return result;
        }

        // 2. Filter: відбирає елементи, які відповідають умові
        public static List<T> Filter<T>(List<T> list, Func<T, bool> predicate)
        {
            var result = new List<T>();
            foreach (var item in list)
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        // 3. Sum: обчислює суму числових значень, обраних за допомогою Func
        public static double Sum<T>(List<T> list, Func<T, double> selector)
        {
            double sum = 0;
            foreach (var item in list)
            {
                sum += selector(item);
            }
            return sum;
        }

        // 4. Max: знаходить максимальне значення обраного поля (where TKey : IComparable<TKey>)
        public static TKey Max<T, TKey>(List<T> list, Func<T, TKey> selector) where TKey : IComparable<TKey>
        {
            if (list == null || list.Count == 0)
                throw new ArgumentException("Колекція не може бути порожньою.");

            TKey maxVal = selector(list[0]);
            for (int i = 1; i < list.Count; i++)
            {
                TKey current = selector(list[i]);
                if (current.CompareTo(maxVal) > 0)
                {
                    maxVal = current;
                }
            }
            return maxVal;
        }

        // 5. Min: знаходить мінімальне значення обраного поля (where TKey : IComparable<TKey>)
        public static TKey Min<T, TKey>(List<T> list, Func<T, TKey> selector) where TKey : IComparable<TKey>
        {
            if (list == null || list.Count == 0)
                throw new ArgumentException("Колекція не може бути порожньою.");

            TKey minVal = selector(list[0]);
            for (int i = 1; i < list.Count; i++)
            {
                TKey current = selector(list[i]);
                if (current.CompareTo(minVal) < 0)
                {
                    minVal = current;
                }
            }
            return minVal;
        }
    }

    // ===================================================
    // 2. ВЛАСНИЙ КЛАС ДЛЯ ТЕСТУВАННЯ
    // ===================================================
    public class Product
    {
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Rating { get; set; }

        public override string ToString() => $"{Name} (Ціна: {Price} грн, Рейтинг: {Rating}/5)";
    }

    // ===================================================
    // 3. ГОЛОВНИЙ МЕТОД ДЕМОНСТРАЦІЇ
    // ===================================================
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // --- ТЕСТ 1: Примітивні типи (int, string) ---
            Console.WriteLine("=== 1. РОБОТА З ПРОСТИМИ ТИПАМИ (int, string) ===");
            List<int> numbers = new List<int> { 12, 5, 27, 8, 41, 19 };

            var squared = FunctionalUtils.Map(numbers, x => x * x);
            var evenNumbers = FunctionalUtils.Filter(numbers, x => x % 2 == 0);
            double totalSum = FunctionalUtils.Sum(numbers, x => x);
            int maxNum = FunctionalUtils.Max(numbers, x => x);
            int minNum = FunctionalUtils.Min(numbers, x => x);

            Console.WriteLine($"Початковий список: {string.Join(", ", numbers)}");
            Console.WriteLine($"Map (квадрати): {string.Join(", ", squared)}");
            Console.WriteLine($"Filter (парні): {string.Join(", ", evenNumbers)}");
            Console.WriteLine($"Sum: {totalSum}");
            Console.WriteLine($"Max: {maxNum}");
            Console.WriteLine($"Min: {minNum}\n");

            // --- ТЕСТ 2: Власний клас (Product) ---
            Console.WriteLine("=== 2. РОБОТА З ВЛАСНИМ КЛАСОМ (Product) ===");
            List<Product> products = new List<Product>
            {
                new Product { Name = "Ноутбук", Price = 35000, Rating = 5 },
                new Product { Name = "Мишка", Price = 850, Rating = 4 },
                new Product { Name = "Клавіатура", Price = 2100, Rating = 4 },
                new Product { Name = "Монітор", Price = 9200, Rating = 5 },
                new Product { Name = "Навушники", Price = 1500, Rating = 3 }
            };

            // Map: Отримуємо список назв
            var productNames = FunctionalUtils.Map(products, p => p.Name.ToUpper());

            // Filter: Товари дешевші за 5000 грн
            var cheapProducts = FunctionalUtils.Filter(products, p => p.Price < 5000);

            // Sum: Загальна вартість усіх товарів
            double totalPrice = FunctionalUtils.Sum(products, p => p.Price);

            // Max/Min: Найвища ціна та найнижчий рейтинг
            double maxPrice = FunctionalUtils.Max(products, p => p.Price);
            int minRating = FunctionalUtils.Min(products, p => p.Rating);

            Console.WriteLine("Усі товари:");
            products.ForEach(p => Console.WriteLine($" - {p}"));

            Console.WriteLine($"\nMap (Назви у верхньому регістрі): {string.Join(", ", productNames)}");

            Console.WriteLine("\nFilter (Товари дешевші 5000 грн):");
            cheapProducts.ForEach(p => Console.WriteLine($" - {p}"));

            Console.WriteLine($"\nSum (Загальна вартість): {totalPrice} грн");
            Console.WriteLine($"Max (Найвища ціна): {maxPrice} грн");
            Console.WriteLine($"Min (Найнижчий рейтинг): {minRating}");
        }
    }
}