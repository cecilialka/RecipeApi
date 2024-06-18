using RecipeApi.Interfaces;

public class NutritionService : INutritionService
{
    private readonly ISpoonacularService _spoonacularService;

    public NutritionService(ISpoonacularService spoonacularService)
    {
        _spoonacularService = spoonacularService;
    }

    public async Task<List<RecipeDetail>> GetRecipesNutritionData(List<Recipe> recipes)
    {
        var recipeDetails = new List<RecipeDetail>();
        foreach (var recipe in recipes)
        {
            var nutrients = await _spoonacularService.GetNutritionByRecipeId(recipe.Id);

            var calories = nutrients.FirstOrDefault(x => x.Name == "Calories")?.Amount ?? 0;
            var proteins = nutrients.FirstOrDefault(x => x.Name == "Protein")?.Amount ?? 0;
            var proteinPercentage = ((proteins * 4) / calories) * 100;

            var fat = nutrients.FirstOrDefault(x => x.Name == "Fat")?.Amount ?? 0;
            var fatPercentage = ((fat * 9) / calories) * 100;

            var carbs = nutrients.FirstOrDefault(x => x.Name == "Carbohydrates")?.Amount ?? 0;
            var carbsPercentage = ((carbs * 4) / calories) * 100;

            var caloriesForServings = calories * recipe.Servings;

            var caloricBreakdown = new CaloricBreakdown
            {
                PercentProtein = Math.Round(proteinPercentage, 2),
                PercentFat = Math.Round(fatPercentage, 2),
                PercentCarbs = Math.Round(carbsPercentage, 2)
            };

            var recipeDetail = new RecipeDetail
            {
                Name = recipe.Title,
                Servings = recipe.Servings,
                CaloriesPerServing = calories,
                TotalCalories = caloriesForServings,
                CaloricBreakdown = caloricBreakdown
            };
            recipeDetails.Add(recipeDetail);
        }

        return recipeDetails;
    }
}