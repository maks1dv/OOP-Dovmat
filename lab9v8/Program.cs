using System;
using System.Collections.Generic;

namespace Lab9Variant8
{
    // ===================================================
    // 1. АБСТРАКТНИЙ БАЗОВИЙ КЛАС
    // ===================================================
    public abstract class Shape
    {
        public string Name { get; set; }

        protected Shape(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Назва фігури не може бути порожньою.");
            
            Name = name;
        }

        public abstract void Draw();
        public abstract double Area();
    }

    // ===================================================
    // 2. ПОХІДНІ КЛАСИ З ВАЛІДАЦІЄЮ ТА ЛОГІКОЮ
    // ===================================================

    // Клас 1: Коло
    public class Circle : Shape
    {
        public double Radius { get; }

        public Circle(string name, double radius) : base(name)
        {
            if (radius <= 0)
                throw new ArgumentException($"Радіус кола повинен бути більше 0. Передано: {radius}");
            
            Radius = radius;
        }

        public override void Draw()
        {
            Console.WriteLine($"[Circle] Рендеринг кола '{Name}' з радіусом R = {Radius}");
        }

        public override double Area()
        {
            return Math.PI * Radius * Radius;
        }
    }

    // Клас 2: Прямокутник
    public class Rectangle : Shape
    {
        public double Width { get; }
        public double Height { get; }

        public Rectangle(string name, double width, double height) : base(name)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException($"Ширина та висота повинні бути більше 0. Передано: {width} x {height}");
            
            Width = width;
            Height = height;
        }

        public override void Draw()
        {
            Console.WriteLine($"[Rectangle] Рендеринг прямокутника '{Name}' з розмірами {Width} x {Height}");
        }

        public override double Area()
        {
            return Width * Height;
        }
    }

    // Клас 3: Трикутник
    public class Triangle : Shape
    {
        public double SideA { get; }
        public double SideB { get; }
        public double SideC { get; }

        public Triangle(string name, double a, double b, double c) : base(name)
        {
            if (a <= 0 || b <= 0 || c <= 0)
                throw new ArgumentException($"Сторони трикутника повинні бути більше 0. Передано: {a}, {b}, {c}");

            if (a + b <= c || a + c <= b || b + c <= a)
                throw new ArgumentException($"Трикутник зі сторонами ({a}, {b}, {c}) не існує в евклідовій геометрії.");

            SideA = a;
            SideB = b;
            SideC = c;
        }

        public override void Draw()
        {
            Console.WriteLine($"[Triangle] Рендеринг трикутника '{Name}' зі сторонами ({SideA}, {SideB}, {SideC})");
        }

        public override double Area()
        {
            double p = (SideA + SideB + SideC) / 2.0;
            return Math.Sqrt(p * (p - SideA) * (p - SideB) * (p - SideC));
        }
    }

    // ===================================================
    // 3. СЕРВІС ДЛЯ РОБОТИ З КОЛЕКЦІЄЮ (ShapeService)
    // ===================================================
    public class ShapeService
    {
        public void RenderAll(List<Shape> shapes)
        {
            Console.WriteLine("=== СТАРТ РЕНДЕРИНГУ КОЛЕКЦІЇ ФІГУР ===\n");

            foreach (var shape in shapes)
            {
                try
                {
                    shape.Draw();
                    Console.WriteLine($"   --> Обчислена площа: {shape.Area():F2} кв. од.\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"   [ПОМИЛКА обробки фігури '{shape.Name}']: {ex.Message}\n");
                }
            }
        }
    }

    // ===================================================
    // 4. ГОЛОВНИЙ МЕТОД (Main)
    // ===================================================
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            List<Shape> shapes = new List<Shape>();

            shapes.Add(new Circle("Мале коло", 5));
            shapes.Add(new Rectangle("Стандартний прямокутник", 10, 4.5));
            shapes.Add(new Triangle("Єгипетський трикутник", 3, 4, 5));

            Console.WriteLine("--- Спроба створення невалідних об'єктів (Тест валідації) ---\n");

            try
            {
                var invalidCircle = new Circle("Помилкове коло", -2);
                shapes.Add(invalidCircle);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"[САПІЙМАНО ВИНОК]: {ex.Message}");
            }

            try
            {
                var invalidTriangle = new Triangle("Неможливий трикутник", 1, 2, 10);
                shapes.Add(invalidTriangle);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"[САПІЙМАНО ВИНОК]: {ex.Message}\n");
            }

            ShapeService service = new ShapeService();
            service.RenderAll(shapes);

            Console.WriteLine("Виконання програми завершено.");
        }
    }
}