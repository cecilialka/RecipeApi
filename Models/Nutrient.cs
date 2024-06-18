namespace RecipeApi.Models;

public class Nutrient {
    public required string Name {get; set;}
    public double Amount {get; set;}
    public required string Unit {get; set;}
    public double PercentOfDailyNeeds {get; set;}
}