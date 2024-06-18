namespace RecipeApi.Interfaces;

public interface IShoppingListService {
    List<string> CreateShoppingList(List<Recipe> recipes);
}