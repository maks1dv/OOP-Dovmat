using System;

namespace OOP_Lab4
{
    public class Rectangle
    {
        private double _width;
        private double _height;

        public double Width
        {
            get => _width;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Ширина повинна бути більшою за 0.");
                }
                _width = value;
            }
        }

        public double Height
        {
            get => _height;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Висота повинна бути більшою за 0.");
                }
                _height = value;
            }
        }

        public double Area => Width * Height;

        public static Rectangle DefaultRectangle => new Rectangle(1.0, 1.0);

        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public static Rectangle operator *(Rectangle rect, double scalar)
        {
            if (rect is null)
                throw new ArgumentNullException(nameof(rect));
            if (scalar <= 0)
                throw new ArgumentException("Скаляр для множення прямокутника повинен бути більшим за 0.");

            return new Rectangle(rect.Width * scalar, rect.Height * scalar);
        }

        public static Rectangle operator *(double scalar, Rectangle rect)
        {
            return rect * scalar;
        }

        public static bool operator ==(Rectangle r1, Rectangle r2)
        {
            if (ReferenceEquals(r1, r2)) return true;
            if (r1 is null || r2 is null) return false;
            return r1.Equals(r2);
        }

        public static bool operator !=(Rectangle r1, Rectangle r2)
        {
            return !(r1 == r2);
        }

        public override bool Equals(object obj)
        {
            if (obj is Rectangle other)
            {
                return Math.Abs(this.Area - other.Area) < 0.000001;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Area.GetHashCode();
        }

        public override string ToString()
        {
            return $"Прямокутник [{Width} x {Height}] (Площа: {Area})";
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Лабораторна робота №4 (Варіант 8: Rectangle) ===\n");

            Rectangle defRect = Rectangle.DefaultRectangle;
            Console.WriteLine($"Статична властивість DefaultRectangle: {defRect}");

            Rectangle r1 = new Rectangle(4.0, 5.0);
            Rectangle r2 = new Rectangle(2.0, 10.0);
            Rectangle r3 = new Rectangle(3.0, 3.0);

            Console.WriteLine("\nСтворено об'єкти:");
            Console.WriteLine($"r1: {r1}");
            Console.WriteLine($"r2: {r2}");
            Console.WriteLine($"r3: {r3}");

            Console.WriteLine("\n--- Перевірка множення на скаляр (*) ---");
            Rectangle scaled = r1 * 2.5;
            Console.WriteLine($"r1 * 2.5 = {scaled}");

            Console.WriteLine("\n--- Перевірка порівняння за площею (==, !=) ---");
            Console.WriteLine($"r1 (площа 20) == r2 (площа 20): {r1 == r2}");
            Console.WriteLine($"r1 (площа 20) == r3 (площа 9):  {r1 == r3}");
            Console.WriteLine($"r1 (площа 20) != r3 (площа 9):  {r1 != r3}");

            Console.WriteLine("\n--- Перевірка валідації (Width, Height > 0) ---");
            try
            {
                Console.WriteLine("Спроба встановити від'ємну ширину...");
                r1.Width = -5;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Спіймано очікуваний виняток: {ex.Message}");
            }

            try
            {
                Console.WriteLine("Спроба створити прямокутник з висотою 0...");
                Rectangle invalidRect = new Rectangle(10, 0);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Спіймано очікуваний виняток: {ex.Message}");
            }
        }
    }
}
