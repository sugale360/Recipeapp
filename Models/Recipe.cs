using System.ComponentModel.DataAnnotations;
namespace recipes.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public int Price { get; set; }
        public string? RecipePage { get; set; }
        public string? Recipes { get; set; }

        internal object ToList()
        {
            throw new NotImplementedException();
        }
    }
}