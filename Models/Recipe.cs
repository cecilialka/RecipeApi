using RecipeApi.Models;

public class Recipe {
    public int Id {get; set;}
    public required string Title {get; set;}
    public int Servings {get; set;}
    public int ReadyInMinutes {get; set;}
    public double HealthScore {get; set;}
    public List<string> Cuisines {get; set;} = new();
    public bool LowFodmap {get; set;}
    public bool GlutenFree {get; set;}
    public bool Vegan {get; set;}
    public bool Vegetarian {get; set;}
    public required string Summary {get; set;}
    public List<Ingredient> ExtendedIngredients {get; set;} = new();
}