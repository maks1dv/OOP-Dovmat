using System;
using System.Collections.Generic;
using System.Linq;

namespace lab16v8
{
    public static class EnumerableExtensions
    {
        // Метод розширення для IEnumerable<DishCategory>
        public static decimal AverageDishPrice(this IEnumerable<DishCategory> categories)
        {
            var allDishes = categories.SelectMany(c => c.Dishes).ToList();
            if (!allDishes.Any()) return 0m;
            return allDishes.Average(d => d.Price);
        }

        // Перевантаження для окремої категорії DishCategory
        public static decimal AverageDishPrice(this DishCategory category)
        {
            if (category?.Dishes == null || !category.Dishes.Any()) return 0m;
            return category.Dishes.Average(d => d.Price);
        }
    }
}
