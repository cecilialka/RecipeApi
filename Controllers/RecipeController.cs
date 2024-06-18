
using Microsoft.AspNetCore.Mvc;
using RecipeApi.Exceptions;
using RecipeApi.Interfaces;
using RecipeApi.Models;

namespace RecipeApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RecipeController : ControllerBase
{
    private readonly ISpoonacularService _spoonacularService;
    private readonly IShoppingListService _shoppingListService;
    private readonly INutritionService _nutritionService;

    public RecipeController(ISpoonacularService spoonacularService, IShoppingListService shoppingListService, INutritionService nutritionService)
    {
        _spoonacularService = spoonacularService;
        _shoppingListService = shoppingListService;
        _nutritionService = nutritionService;
    }

    ///<summary>
    ///Get a random number of recipes, filtered by tags (cuisines, diets, intolerances)
    ///</summary>
    /// <param name="number" example="5">Number of recipes to get</param>
    /// <param name="tags" example="vegetarian, italian">Tags to filter recipes</param>
    /// <response code="200">Recipes retrieved</response>
    /// <response code="404">No matching recipes</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<Recipe>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<List<Recipe>>> GetRandomRecipes([FromQuery] int number, [FromQuery] List<string> tags)
    {
        try
        {
            var recipes = await _spoonacularService.GetRandomRecipes(number, tags);
            return Ok(recipes);
        }
        catch (ItemNotFoundException ex)
        {
            return NotFound(ex.Message);
        }

    }

    ///<summary>
    ///Get recipe by id
    ///</summary>
    /// <param name="id" example="633884">Recipe Id</param>
    /// <response code="200">Recipe retrieved</response>
    /// <response code="404">No recipe with given Id</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Recipe), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Recipe>> GetRecipe(int id)
    {
        try
        {
            var recipe = await _spoonacularService.GetRecipe(id);
            return Ok(recipe);
        }
        catch (ItemNotFoundException ex)
        {
            return NotFound(ex.Message);
        }

    }

    ///<summary>
    ///Get shopping list from list of recipes
    ///</summary>
    /// <param name="recipeIds" example="[633884, 631807, 651922, 638235, 652722]">Recipe Ids</param>
    /// <response code="200">Recipes retrieved</response>
    /// <response code="404">No matching recipes</response>
    [HttpGet("shopping-list")]
    [ProducesResponseType(typeof(List<string>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<List<string>>> GetShoppingList([FromQuery] List<int> recipeIds)
    {
        var recipes = await _spoonacularService.GetRecipes(recipeIds);
        var shoppingList = _shoppingListService.CreateShoppingList(recipes);
        return Ok(shoppingList);
    }

    ///<summary>
    ///Get nutrition details for a list of recipes
    ///</summary>
    /// <param name="recipeIds" example="[633884, 631807, 651922, 638235, 652722]">Recipe Ids</param>
    /// <response code="200">Recipe details retrieved</response>
    /// <response code="404">No matching recipes</response>
    [HttpGet("nutrition-details")]
    [ProducesResponseType(typeof(List<RecipeDetail>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<List<RecipeDetail>>> GetRecipesNutritionData([FromQuery] List<int> recipeIds)
    {
        var recipes = await _spoonacularService.GetRecipes(recipeIds);
        var recipeDetails = await _nutritionService.GetRecipesNutritionData(recipes);
        return Ok(recipeDetails);
    }

    // [HttpGet("{id}/nutrition")]
    // public async Task<ActionResult<List<Nutrient>>> GetRecipeNutritionData(int id)
    // {
    //     var nutrients = await _spoonacularService.GetNutritionByRecipeId(id);
    //     return Ok(nutrients);
    // }
}
//633884, 631807, 651922, 638235, 652722
