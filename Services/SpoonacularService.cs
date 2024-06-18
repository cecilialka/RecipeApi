using RecipeApi.Exceptions;
using RecipeApi.Interfaces;
using RecipeApi.Models;

public class SpoonacularService : ISpoonacularService {
    private readonly IHttpClientFactory _factory;

    public SpoonacularService(IHttpClientFactory factory) 
    {
        _factory = factory;
    }

    public async Task<List<Recipe>> GetRandomRecipes(int number, List<string> tags) 
    {
        var client = _factory.CreateClient("spoonacular");
        var recipeTags = string.Join(",", tags);

        var response = await client.GetFromJsonAsync<RecipesResponse>($"/recipes/random?number={number}&include-tags={recipeTags.ToLower()}");

        if(response == null || !response.Recipes.Any()) {
            throw new ItemNotFoundException("No matching recipes was found.");
        }

        return response.Recipes.OrderByDescending(x => x.HealthScore).ToList();
    }

    public async Task<List<Recipe>> GetRecipes(List<int> recipeIds) 
    {
        var client = _factory.CreateClient("spoonacular");
        var ids = string.Join(",", recipeIds);

        var recipes = await client.GetFromJsonAsync<List<Recipe>>($"/recipes/informationBulk?ids={ids}");

        if(recipes == null || !recipes.Any()) {
            throw new ItemNotFoundException("No matching recipes was found.");
        }

        return recipes;
    }

    public async Task<Recipe> GetRecipe(int recipeId) 
    {
        var client = _factory.CreateClient("spoonacular");
        var recipe = await client.GetFromJsonAsync<Recipe>($"/recipes/{recipeId}/information");

        if(recipe == null) {
            throw new ItemNotFoundException($"Recipe with Id: {recipeId} was not found.");
        }

        return recipe;
    }

    public async Task<List<Nutrient>> GetNutritionByRecipeId(int recipeId) {
        var client = _factory.CreateClient("spoonacular");

        var response = await client.GetFromJsonAsync<NutrientResponse>($"recipes/{recipeId}/nutritionWidget.json");

        if(response == null || !response.Nutrients.Any()) {
            throw new ItemNotFoundException($"No nutrients were found for Recipe with Id: {recipeId}");
        }
        var nutrients = response.Nutrients;
        return nutrients;
    }
}
    