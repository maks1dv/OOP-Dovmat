using System;

namespace Lab3
{
    public class CustomResource : IDisposable
    {
        private bool _disposed = false;
        private bool _isResourceAllocated;

        public string ResourceName { get; set; }

        public CustomResource(string resourceName)
        {
            ResourceName = resourceName;
            _isResourceAllocated = true;
            Console.WriteLine($"[КОНСТРУКТОР] Ресурс '{ResourceName}' виділено.");
        }

        public void DoWork()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(CustomResource), $"Неможливо використати ресурс '{ResourceName}', оскільки його вже вивільнено.");
            }

            Console.WriteLine($"[РОБОТА] Виконується дія з ресурсом '{ResourceName}'...");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Console.WriteLine($"[DISPOSE] Звільнення КЕРОВАНИХ ресурсів для '{ResourceName}'.");
                }

                if (_isResourceAllocated)
                {
                    Console.WriteLine($"[DISPOSE] Звільнення НЕКЕРОВАНИХ ресурсів для '{ResourceName}'.");
                    _isResourceAllocated = false;
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CustomResource()
        {
            Console.WriteLine($"[ФІНАЛІЗАТОР] Спрацював деструктор для '{ResourceName}'.");
            Dispose(false);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== 1. Явне використання конструкції using ===");
            DemonstrateUsingPattern();

            Console.WriteLine("\n=== 2. Явний виклик Dispose() без using ===");
            DemonstrateExplicitDispose();

            Console.WriteLine("\n=== 3. Демонстрація роботи Фіналізатора (GC.Collect) ===");
            DemonstrateFinalizer();

            Console.WriteLine("\n=== Демонстрацію завершено ===");
        }

        static void DemonstrateUsingPattern()
        {
            using (var res1 = new CustomResource("Resource_Using"))
            {
                res1.DoWork();
            }
        }

        static void DemonstrateExplicitDispose()
        {
            CustomResource res2 = new CustomResource("Resource_Manual");
            try
            {
                res2.DoWork();
            }
            finally
            {
                res2.Dispose();
            }
        }

        static void DemonstrateFinalizer()
        {
            CreateUnmanagedObject();

            Console.WriteLine("Примусовий виклик збирача сміття (GC.Collect)...");

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        static void CreateUnmanagedObject()
        {
            var res3 = new CustomResource("Resource_Forgotten");
            res3.DoWork();
        }
    }
}
