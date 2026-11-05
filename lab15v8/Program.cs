using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace lab15v8
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Заповнення тестовими даними (4 команди, 15 гравців)
            var teams = new List<Team>
            {
                new Team { Id = 1, Name = "Lions", City = "Львів" },
                new Team { Id = 2, Name = "Eagles", City = "Київ" },
                new Team { Id = 3, Name = "Tigers", City = "Одеса" },
                new Team { Id = 4, Name = "Bears", City = "Харків" }
            };

            var players = new List<Player>
            {
                new Player { Id = 1, Name = "Максим Довмат", TeamId = 1, Age = 21 },
                new Player { Id = 2, Name = "Олександр Коваль", TeamId = 1, Age = 24 },
                new Player { Id = 3, Name = "Іван Петренко", TeamId = 1, Age = 28 },
                new Player { Id = 4, Name = "Дмитро Сидоренко", TeamId = 1, Age = 23 },
                new Player { Id = 5, Name = "Андрій Мельник", TeamId = 2, Age = 22 },
                new Player { Id = 6, Name = "Сергій Бойко", TeamId = 2, Age = 29 },
                new Player { Id = 7, Name = "Тарас Шевченко", TeamId = 2, Age = 26 },
                new Player { Id = 8, Name = "Василь Бондар", TeamId = 3, Age = 20 },
                new Player { Id = 9, Name = "Олег Ткаченко", TeamId = 3, Age = 31 },
                new Player { Id = 10, Name = "Микола Кравченко", TeamId = 3, Age = 25 },
                new Player { Id = 11, Name = "Юрій Олійник", TeamId = 4, Age = 19 },
                new Player { Id = 12, Name = "Артем Поліщук", TeamId = 4, Age = 27 },
                new Player { Id = 13, Name = "Роман Лисенко", TeamId = 4, Age = 33 },
                new Player { Id = 14, Name = "Віктор Мороз", TeamId = 1, Age = 22 },
                new Player { Id = 15, Name = "Павло Руденко", TeamId = 2, Age = 24 }
            };

            Console.WriteLine("=== ЛАБОРАТОРНА РОБОТА №15: LINQ В ДІЇ (ВАРІАНТ 8 — СПОРТИВНА ЛІГА) ===\n");

            var sw = new Stopwatch();

            // -------------------------------------------------------------
            // Запит 1: Гравці віком до 25 років з команди "Lions"
            // -------------------------------------------------------------
            Console.WriteLine("--- ЗАПИТ 1: Гравці віком < 25 з команди 'Lions' ---");

            sw.Restart();
            var q1Method = players
                .Join(teams, p => p.TeamId, t => t.Id, (p, t) => new { Player = p, Team = t })
                .Where(x => x.Team.Name == "Lions" && x.Player.Age < 25)
                .Select(x => x.Player)
                .ToList();
            sw.Stop();
            long t1Method = sw.ElapsedTicks;

            sw.Restart();
            var q1Query = (from p in players
                           join t in teams on p.TeamId equals t.Id
                           where t.Name == "Lions" && p.Age < 25
                           select p).ToList();
            sw.Stop();
            long t1Query = sw.ElapsedTicks;

            foreach (var p in q1Method)
            {
                Console.WriteLine($" - {p.Name}, вік: {p.Age}");
            }
            Console.WriteLine($"[Час] Method Syntax: {t1Method} ticks | Query Syntax: {t1Query} ticks\n");

            // -------------------------------------------------------------
            // Запит 2: Об'єднання гравців з їхніми командами
            // -------------------------------------------------------------
            Console.WriteLine("--- ЗАПИТ 2: Об'єднання гравців з командами ---");

            sw.Restart();
            var q2Method = players
                .Join(teams, p => p.TeamId, t => t.Id, (p, t) => new { PlayerName = p.Name, TeamName = t.Name })
                .ToList();
            sw.Stop();
            long t2Method = sw.ElapsedTicks;

            sw.Restart();
            var q2Query = (from p in players
                           join t in teams on p.TeamId equals t.Id
                           select new { PlayerName = p.Name, TeamName = t.Name }).ToList();
            sw.Stop();
            long t2Query = sw.ElapsedTicks;

            foreach (var item in q2Method.Take(5))
            {
                Console.WriteLine($" - Гравець: {item.PlayerName,-18} | Команда: {item.TeamName}");
            }
            Console.WriteLine($"... (усього {q2Method.Count} записів)");
            Console.WriteLine($"[Час] Method Syntax: {t2Method} ticks | Query Syntax: {t2Query} ticks\n");

            // -------------------------------------------------------------
            // Запит 3: Середній вік гравців у кожній команді
            // -------------------------------------------------------------
            Console.WriteLine("--- ЗАПИТ 3: Середній вік гравців у кожній команді ---");

            sw.Restart();
            var q3Method = teams
                .Select(t => new
                {
                    TeamName = t.Name,
                    AvgAge = players.Where(p => p.TeamId == t.Id).Average(p => p.Age)
                })
                .ToList();
            sw.Stop();
            long t3Method = sw.ElapsedTicks;

            sw.Restart();
            var q3Query = (from t in teams
                           join p in players on t.Id equals p.TeamId into teamPlayers
                           select new
                           {
                               TeamName = t.Name,
                               AvgAge = teamPlayers.Average(p => p.Age)
                           }).ToList();
            sw.Stop();
            long t3Query = sw.ElapsedTicks;

            foreach (var item in q3Method)
            {
                Console.WriteLine($" - Команда: {item.TeamName,-8} | Середній вік: {item.AvgAge:F1} років");
            }
            Console.WriteLine($"[Час] Method Syntax: {t3Method} ticks | Query Syntax: {t3Query} ticks\n");

            // -------------------------------------------------------------
            // Запит 4: Найстарший гравець
            // -------------------------------------------------------------
            Console.WriteLine("--- ЗАПИТ 4: Найстарший гравець ---");

            sw.Restart();
            var q4Method = players.OrderByDescending(p => p.Age).FirstOrDefault();
            sw.Stop();
            long t4Method = sw.ElapsedTicks;

            sw.Restart();
            var q4Query = (from p in players
                           orderby p.Age descending
                           select p).FirstOrDefault();
            sw.Stop();
            long t4Query = sw.ElapsedTicks;

            if (q4Method != null)
            {
                Console.WriteLine($" - Найстарший гравець: {q4Method.Name}, вік: {q4Method.Age} років");
            }
            Console.WriteLine($"[Час] Method Syntax: {t4Method} ticks | Query Syntax: {t4Query} ticks\n");

            Console.WriteLine("=== ВИКОНАННЯ LINQ-ЗАПИТІВ ЗАВЕРШЕНО ===");
        }
    }
}
