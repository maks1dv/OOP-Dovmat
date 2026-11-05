using System;
using System.Collections.Generic;
using System.Linq;

namespace lab16v8
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Наповнення тестовими даними (2 меню, 5 категорій, 16 страв)
            var menus = new List<RestaurantMenu>
            {
                new RestaurantMenu
                {
                    Id = 1,
                    Name = "Основне меню",
                    Categories = new List<DishCategory>
                    {
                        new DishCategory
                        {
                            Id = 1,
                            Name = "Перші страви",
                            Dishes = new List<Dish>
                            {
                                new Dish { Id = 101, Name = "Борщ український", Price = 120.00m },
                                new Dish { Id = 102, Name = "Суп грибний", Price = 110.00m },
                                new Dish { Id = 103, Name = "Солянка", Price = 145.00m }
                            }
                        },
                        new DishCategory
                        {
                            Id = 2,
                            Name = "Основні страви",
                            Dishes = new List<Dish>
                            {
                                new Dish { Id = 104, Name = "Стейк з яловичини", Price = 380.00m },
                                new Dish { Id = 105, Name = "Котлета по-київськи", Price = 190.00m },
                                new Dish { Id = 106, Name = "Вареники з картоплею", Price = 95.00m },
                                new Dish { Id = 107, Name = "Ребра свинячі на грилі", Price = 260.00m }
                            }
                        },
                        new DishCategory
                        {
                            Id = 3,
                            Name = "Десерти",
                            Dishes = new List<Dish>
                            {
                                new Dish { Id = 108, Name = "Тірамісу", Price = 130.00m },
                                new Dish { Id = 109, Name = "Чізкейк", Price = 125.00m },
                                new Dish { Id = 110, Name = "Морозиво асорті", Price = 75.00m }
                            }
                        }
                    }
                },
                new RestaurantMenu
                {
                    Id = 2,
                    Name = "Сезонне меню",
                    Categories = new List<DishCategory>
                    {
                        new DishCategory
                        {
                            Id = 4,
                            Name = "Салати",
                            Dishes = new List<Dish>
                            {
                                new Dish { Id = 111, Name = "Салат Цезар", Price = 165.00m },
                                new Dish { Id = 112, Name = "Грецький салат", Price = 140.00m },
                                new Dish { Id = 113, Name = "Салат з тунцем", Price = 185.00m }
                            }
                        },
                        new DishCategory
                        {
                            Id = 5,
                            Name = "Напої",
                            Dishes = new List<Dish>
                            {
                                new Dish { Id = 114, Name = "Лимонад домашній", Price = 65.00m },
                                new Dish { Id = 115, Name = "Мохіто безалкогольний", Price = 85.00m },
                                new Dish { Id = 116, Name = "Кава Лате", Price = 55.00m }
                            }
                        }
                    }
                }
            };

            Console.WriteLine("=== ЛАБОРАТОРНА РОБОТА №16: SELECTMANY, ПАГІНАЦІЯ ТА CUSTOM EXTENSIONS (ВАРІАНТ 8) ===\n");

            // -------------------------------------------------------------
            // 1. SelectMany: Отримати всі страви з усіх категорій всіх меню в одному списку
            // -------------------------------------------------------------
            Console.WriteLine("--- 1. SELECTMANY: Усі страви з усіх категорій усіх меню ---");
            var allDishes = menus
                .SelectMany(m => m.Categories)
                .SelectMany(c => c.Dishes)
                .ToList();

            Console.WriteLine($"Усього знайдено страв у плоскому списку: {allDishes.Count}");
            foreach (var dish in allDishes)
            {
                Console.WriteLine($" - [{dish.Id}] {dish.Name,-25} : {dish.Price:C}");
            }

            // -------------------------------------------------------------
            // 2. Пагінація: Вивести всі категорії страв по 2 категорії на сторінку
            // -------------------------------------------------------------
            Console.WriteLine("\n--- 2. ПАГІНАЦІЯ: Категорії страв по 2 на сторінку ---");
            var allCategories = menus.SelectMany(m => m.Categories).ToList();
            int pageSize = 2;
            int totalPages = (int)Math.Ceiling((double)allCategories.Count / pageSize);

            for (int page = 1; page <= totalPages; page++)
            {
                var pageCategories = allCategories
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                Console.WriteLine($"\n[Сторінка {page} з {totalPages}]");
                foreach (var cat in pageCategories)
                {
                    Console.WriteLine($" * Категорія #{cat.Id}: {cat.Name} (Кількість страв: {cat.Dishes.Count})");
                }
            }

            // -------------------------------------------------------------
            // 3. Custom Extension: AverageDishPrice()
            // -------------------------------------------------------------
            Console.WriteLine("\n--- 3. CUSTOM EXTENSION: Середня ціна страв (AverageDishPrice) ---");
            
            foreach (var category in allCategories)
            {
                decimal avgPriceCategory = category.AverageDishPrice();
                Console.WriteLine($" - Середня ціна у категорії '{category.Name}': {avgPriceCategory:F2} ₴");
            }

            decimal avgPriceTotal = allCategories.AverageDishPrice();
            Console.WriteLine($"\n[Загальний підсумок] Середня ціна по всіх категоріях: {avgPriceTotal:F2} ₴");

            Console.WriteLine("\n=== ВИКОНАННЯ ЛАБОРАТОРНОЇ РОБОТИ №16 ЗАВЕРШЕНО ===");
        }
    }
}
