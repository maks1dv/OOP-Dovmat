using System;

namespace Lab7v8
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }

        public Account(string accountNumber, decimal balance)
        {
            AccountNumber = accountNumber;
            Balance = balance;
        }

        public virtual void CalculateInterest()
        {
            Console.WriteLine($"[Account] Базовий розрахунок відсотків для рахунку {AccountNumber}.");
        }
    }

    public class SavingsAccount : Account
    {
        public decimal InterestRate { get; set; }

        public SavingsAccount(string accountNumber, decimal balance, decimal interestRate)
            : base(accountNumber, balance)
        {
            InterestRate = interestRate;
        }

        public override void CalculateInterest()
        {
            decimal interest = Balance * InterestRate;
            Console.WriteLine($"[SavingsAccount (override)] Відсотки для рахунку {AccountNumber}: {interest} (Ставка: {InterestRate:P}).");
        }
    }

    public class CheckingAccount : Account
    {
        public decimal OverdraftFee { get; set; }

        public CheckingAccount(string accountNumber, decimal balance, decimal overdraftFee)
            : base(accountNumber, balance)
        {
            OverdraftFee = overdraftFee;
        }

        public new void CalculateInterest()
        {
            Console.WriteLine($"[CheckingAccount (new)] Рахунок {AccountNumber} не нараховує відсотки, але має комісію за овердрафт {OverdraftFee}.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            SavingsAccount mySavings = new SavingsAccount("SA-123", 1000m, 0.05m);
            CheckingAccount myChecking = new CheckingAccount("CA-456", 500m, 20m);

            Account refToSavings = mySavings;
            Account refToChecking = myChecking;

            Console.WriteLine("=== 1. ВИКЛИК ЧЕРЕЗ ПОСИЛАННЯ БАЗОВОГО КЛАСУ (Account) ===\n");

            Console.Write("refToSavings.CalculateInterest():  ");
            refToSavings.CalculateInterest();

            Console.Write("refToChecking.CalculateInterest(): ");
            refToChecking.CalculateInterest();

            Console.WriteLine("\n=== 2. ВИКЛИК ЧЕРЕЗ ПОСИЛАННЯ ПОХІДНИХ КЛАСІВ (з явним приведенням) ===\n");

            Console.Write("((SavingsAccount)refToSavings).CalculateInterest():  ");
            ((SavingsAccount)refToSavings).CalculateInterest();

            Console.Write("((CheckingAccount)refToChecking).CalculateInterest(): ");
            ((CheckingAccount)refToChecking).CalculateInterest();

            Console.WriteLine("\nНатисніть будь-яку клавішу для завершення...");
            Console.ReadKey();
        }
    }
}