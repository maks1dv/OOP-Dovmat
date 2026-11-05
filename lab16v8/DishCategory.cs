using System.Collections.Generic;

namespace lab16v8
{
    public class DishCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Dish> Dishes { get; set; } = new List<Dish>();
    }
}
