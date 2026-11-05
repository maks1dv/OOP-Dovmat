using System;
using System.Collections.Generic;

namespace lab14v8
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== ЛАБОРАТОРНА РОБОТА №14: ОБРОБКА ТРАНЗАКЦІЙ (ВАРІАНТ 8) ===");

            // 1. HashSet<T>: Уникальные ID
            Console.WriteLine("\n--- 1. ДЕМОНСТРАЦІЯ HASHSET<T> (Перевірка унікальності) ---");
            HashSet<string> uniqueIds = new HashSet<string>();

            string id1 = "TXN-1001";
            string id2 = "TXN-1002";
            string id3 = "TXN-1003";

            Console.WriteLine($"Додавання ID {id1}: {uniqueIds.Add(id1)}");
            Console.WriteLine($"Додавання ID {id2}: {uniqueIds.Add(id2)}");
            Console.WriteLine($"Додавання ID {id3}: {uniqueIds.Add(id3)}");
            Console.WriteLine($"Спроба повторного додавання {id1}: {uniqueIds.Add(id1)} (Дублікат відхилено)");
            Console.WriteLine($"Перевірка наявності {id2} через Contains(): {uniqueIds.Contains(id2)}");
            Console.WriteLine($"Всього унікальних ID: {uniqueIds.Count}");

            // 2. Queue<T>: Очередь выполнения (FIFO)
            Console.WriteLine("\n--- 2. ДЕМОНСТРАЦІЯ QUEUE<T> (Черга обробки FIFO) ---");
            Queue<Transaction> queue = new Queue<Transaction>();

            queue.Enqueue(new Transaction { Id = id1, Sender = "Максим", Receiver = "Олена", Amount = 1500.00m });
            queue.Enqueue(new Transaction { Id = id2, Sender = "Іван", Receiver = "Петро", Amount = 750.50m });
            queue.Enqueue(new Transaction { Id = id3, Sender = "Анна", Receiver = "Марія", Amount = 3200.00m });

            Console.WriteLine($"Транзакцій у черзі: {queue.Count}");
            Console.WriteLine($"Перша в черзі (Peek): {queue.Peek()}");

            // 3. Stack<T>: История отмены (LIFO)
            Console.WriteLine("\n--- 3. ОБРОБКА ЧЕРГИ ТА НАПОВНЕННЯ STACK<T> (LIFO) ---");
            Stack<Transaction> undoStack = new Stack<Transaction>();

            while (queue.Count > 0)
            {
                Transaction current = queue.Dequeue();
                Console.WriteLine($"[Оброблено Dequeue]: {current}");
                undoStack.Push(current);
            }

            Console.WriteLine($"Записів в історії відміни (Stack): {undoStack.Count}");
            Console.WriteLine($"Остання оброблена (Peek): {undoStack.Peek()}");

            // 4. Отмена операции (Pop)
            Console.WriteLine("\n--- 4. ДЕМОНСТРАЦІЯ СКАСУВАННЯ ТРАНЗАКЦІЇ (UNDO / POP) ---");
            if (undoStack.Count > 0)
            {
                Transaction undone = undoStack.Pop();
                Console.WriteLine($"[СКАСОВАНО (Pop)]: {undone}");
                uniqueIds.Remove(undone.Id);
                Console.WriteLine($"ID {undone.Id} вилучено з HashSet унікальних ID.");
            }

            Console.WriteLine($"Залишилось у стеці відміни: {undoStack.Count}");
            Console.WriteLine("=== ОБРОБКУ УСПІШНО ЗАВЕРШЕНО ===");
        }
    }
}
