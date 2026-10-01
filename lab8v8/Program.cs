using System;
using System.Collections.Generic;

namespace lab8v8
{
    // Базовий клас
    public class Transaction
    {
        public decimal Amount { get; set; }

        public Transaction(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сума повинна бути більшою за 0.", nameof(amount));

            Amount = amount;
        }

        // Віртуальний метод
        public virtual void Execute()
        {
            Console.WriteLine($"Базова транзакція: {Amount:C}");
        }
    }

    // Похідний клас 1: Поповнення
    public class Deposit : Transaction
    {
        public string DestinationAccount { get; set; }

        public Deposit(decimal amount, string destinationAccount) : base(amount)
        {
            DestinationAccount = destinationAccount ?? throw new ArgumentNullException(nameof(destinationAccount));
        }

        public override void Execute()
        {
            Console.WriteLine($"[ПОПОВНЕННЯ] Рахунок: {DestinationAccount} | Сума: {Amount:C}");
        }
    }

    // Похідний клас 2: Зняття
    public class Withdrawal : Transaction
    {
        public string SourceAccount { get; set; }

        public Withdrawal(decimal amount, string sourceAccount) : base(amount)
        {
            SourceAccount = sourceAccount ?? throw new ArgumentNullException(nameof(sourceAccount));
        }

        public override void Execute()
        {
            Console.WriteLine($"[ЗНЯТТЯ] Рахунок: {SourceAccount} | Сума: {Amount:C}");
        }
    }

    // Похідний клас 3: Переказ
    public class Transfer : Transaction
    {
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }

        public Transfer(decimal amount, string fromAccount, string toAccount) : base(amount)
        {
            FromAccount = fromAccount ?? throw new ArgumentNullException(nameof(fromAccount));
            ToAccount = toAccount ?? throw new ArgumentNullException(nameof(toAccount));
        }

        public override void Execute()
        {
            Console.WriteLine($"[ПЕРЕКАЗ] З: {FromAccount} -> На: {ToAccount} | Сума: {Amount:C}");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Колекція об'єктів базового типу
            List<Transaction> transactions = new List<Transaction>
            {
                new Deposit(1500.00m, "UA1234567890"),
                new Withdrawal(300.50m, "UA1234567890"),
                new Transfer(750.00m, "UA1234567890", "UA0987654321"),
                new Deposit(2000.00m, "UA0987654321")
            };

            Console.WriteLine("=== Виконання транзакцій ===\n");

            decimal totalAmount = 0;

            // Виклик методів та агрегація
            foreach (var transaction in transactions)
            {
                transaction.Execute();
                totalAmount += transaction.Amount;
            }

            Console.WriteLine("\n=== Результати ===");
            Console.WriteLine($"Кількість: {transactions.Count}");
            Console.WriteLine($"Загальна сума: {totalAmount:C}");
        }
    }
}