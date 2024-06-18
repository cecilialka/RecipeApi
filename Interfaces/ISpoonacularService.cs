using RecipeApi.Models;

namespace RecipeApi.Interfaces;

public interface ISpoonacularService {
    Task<List<Recipe>> GetRecipes(List<int> recipeIds);
    Task<List<Recipe>> GetRandomRecipes(int number, List<string> tags);
    Task<Recipe> GetRecipe(int recipeId);
    Task<List<Nutrient>> GetNutritionByRecipeId(int recipeId);
}