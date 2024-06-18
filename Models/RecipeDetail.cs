public class RecipeDetail {
    public required string Name {get; set;}
    public int Servings {get; set;}
    public double CaloriesPerServing {get; set;}
    public double TotalCalories {get; set;}
    public required CaloricBreakdown CaloricBreakdown {get; set;}
}