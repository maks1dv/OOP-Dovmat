using System;
using System.Collections.Generic;
using System.Globalization;

namespace Lab10Variant8
{
    // ===================================================
    // 1. ІНТЕРФЕЙС ТА ЙОГО РЕАЛІЗАЦІЇ (IParser<T>)
    // ===================================================

    public interface IParser<T>
    {
        T Parse(string input);
    }

    // Реалізація 1: Парсер дати у форматі "yyyy-MM-dd"
    public class DateParser : IParser<DateTime>
    {
        public DateTime Parse(string input)
        {
            if (DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                Console.WriteLine($"[DateParser] Успішно розпізнано дату: {result:dd.MM.yyyy}");
                return result;
            }
            
            Console.WriteLine($"[DateParser] Не вдалося розпізнати дату з рядка: '{input}'");
            return DateTime.MinValue;
        }
    }

    // Реалізація 2: Парсер часу у форматі "HH:mm:ss"
    public class TimeParser : IParser<DateTime>
    {
        public DateTime Parse(string input)
        {
            if (DateTime.TryParseExact(input, "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                Console.WriteLine($"[TimeParser] Успішно розпізнано час: {result:HH:mm:ss}");
                return result;
            }

            Console.WriteLine($"[TimeParser] Не вдалося розпізнати час з рядка: '{input}'");
            return DateTime.MinValue;
        }
    }

    // ===================================================
    // 2. АБСТРАКТНИЙ КЛАС ТА ПОХІДНІ КЛАСИ (Renderer)
    // ===================================================

    public abstract class Renderer
    {
        // Абстрактні методи (обов'язкові для реалізації в похідних класах)
        public abstract void RenderHeader();
        public abstract void RenderBody();

        // Конкретний метод із загальною логікою
        public void RenderFooter()
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("[Renderer] Завершення генерації документа (Footer).");
            Console.WriteLine("--------------------------------------------------\n");
        }
    }

    // Похідний клас 1: HTML рендерер
    public class HtmlRenderer : Renderer
    {
        public override void RenderHeader()
        {
            Console.WriteLine("<html>\n<head><title>HTML Document</title></head>\n<body>");
            Console.WriteLine("  <h1><h1>Заголовок HTML-документа</h1>");
        }

        public override void RenderBody()
        {
            Console.WriteLine("  <p>Основний вміст сторінки у форматі HTML.</p>\n</body>\n</html>");
        }
    }

    // Похідний клас 2: Markdown рендерер
    public class MarkdownRenderer : Renderer
    {
        public override void RenderHeader()
        {
            Console.WriteLine("# Заголовок Markdown-документа");
            Console.WriteLine("===============================");
        }

        public override void RenderBody()
        {
            Console.WriteLine("Текст документа у форматі **Markdown** із *курсивом* та списками.");
        }
    }

    // ===================================================
    // 3. ГОЛОВНИЙ МЕТОД (Демонстрація поліморфізму)
    // ===================================================

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== 1. ДЕМОНСТРАЦІЯ РОБОТИ З ІНТЕРФЕЙСОМ IParser<DateTime> ===\n");

            // Колекція об'єктів типу інтерфейсу
            List<IParser<DateTime>> parsers = new List<IParser<DateTime>>
            {
                new DateParser(),
                new TimeParser()
            };

            string[] testInputs = { "2026-10-22", "14:30:00", "invalid-date" };

            foreach (var parser in parsers)
            {
                Console.WriteLine($"--- Виклик для {parser.GetType().Name} ---");
                foreach (var input in testInputs)
                {
                    parser.Parse(input);
                }
                Console.WriteLine();
            }

            Console.WriteLine("===========================================================");
            Console.WriteLine("=== 2. ДЕМОНСТРАЦІЯ РОБОТИ З АБСТРАКТНИМ КЛАСОМ Renderer ===\n");

            // Колекція об'єктів типу абстрактного класу
            List<Renderer> renderers = new List<Renderer>
            {
                new HtmlRenderer(),
                new MarkdownRenderer()
            };

            foreach (var renderer in renderers)
            {
                Console.WriteLine($"--- Відображення через {renderer.GetType().Name} ---");
                renderer.RenderHeader(); // Поліморфний виклик абстрактного методу
                renderer.RenderBody();   // Поліморфний виклик абстрактного методу
                renderer.RenderFooter(); // Виклик конкретного методу базового класу
            }

            Console.WriteLine("Роботу програми завершено.");
        }
    }
}