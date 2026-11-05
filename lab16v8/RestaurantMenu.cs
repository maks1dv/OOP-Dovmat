using System.Collections.Generic;

namespace lab16v8
{
    public class RestaurantMenu
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<DishCategory> Categories { get; set; } = new List<DishCategory>();
    }
}
