using System;
using System.Linq;

namespace lab12v8
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            IRepository<TaskItem> repository = new InMemoryRepository<TaskItem>();

            Console.WriteLine("=== Демонстрація CRUD для TaskItem ===");

            repository.Add(new TaskItem { Id = 1, Title = "Лаб 11", Description = "Template Method", IsCompleted = true });
            repository.Add(new TaskItem { Id = 2, Title = "Лаб 12", Description = "Generics & Tests", IsCompleted = false });

            Console.WriteLine("\n--- Список після додавання: ---");
            foreach (var task in repository.GetAll())
            {
                Console.WriteLine($"[{task.Id}] {task.Title}: {task.Description} (Виконано: {task.IsCompleted})");
            }

            var taskToUpdate = repository.GetById(2);
            if (taskToUpdate != null)
            {
                taskToUpdate.IsCompleted = true;
                repository.Update(taskToUpdate);
                Console.WriteLine($"\nОновлено Id=2: IsCompleted = {repository.GetById(2)?.IsCompleted}");
            }

            repository.Delete(1);
            Console.WriteLine($"\nКількість залишків після видалення Id=1: {repository.GetAll().Count()}");
        }
    }
}
