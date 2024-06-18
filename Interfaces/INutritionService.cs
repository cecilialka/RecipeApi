public interface INutritionService
{
    Task<List<RecipeDetail>> GetRecipesNutritionData(List<Recipe> recipes);
}