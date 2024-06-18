using RecipeApi.Helpers;
using RecipeApi.Interfaces;
using RecipeApi.Models;

public class ShoppingListService : IShoppingListService
{

    public List<string> CreateShoppingList(List<Recipe> recipes)
    {
        var shoppingList = new Dictionary<string, ShoppingListItem>();

        foreach (var recipe in recipes)
        {
            foreach (var ingredient in recipe.ExtendedIngredients)
            {
                var key = ingredient.Name.ToLower();
                var unit = ingredient.Measures.Metric.UnitShort;
                var amount = ingredient.Measures.Metric.Amount;

                ProcessIngredient(shoppingList, ingredient, key, unit, amount);
            }
        }

        var shoppingListItems = shoppingList.Values.Select(x => $"{Math.Round(x.Amount)} {x.Unit} {x.Item}").ToList();
        return shoppingListItems;
    }

    private void ProcessIngredient(Dictionary<string, ShoppingListItem> list, Ingredient ingredient, string key, string unit, double amount)
    {
        if (list.ContainsKey(key))
        {
            var existingItem = list[key];

            if (existingItem.Unit == unit)
            {
                existingItem.Amount += amount;
            }
            else
            {
                try
                {
                    var convertedAmount = UnitConverter.Convert(amount, unit, existingItem.Unit);
                    existingItem.Amount += convertedAmount;
                }
                catch (InvalidOperationException)
                {
                    key = $"{ingredient.Name.ToLower()} ({unit})";
                    if (list.ContainsKey(key))
                    {
                        list[key].Amount += amount;
                    }
                    else
                    {
                        AddNewItem(list, ingredient, key, amount, unit);
                    }
                }
            }
        }
        else
        {
            AddNewItem(list, ingredient, key, amount, unit);
        }
    }

    private void AddNewItem(Dictionary<string, ShoppingListItem> list, Ingredient ingredient, string key, double amount, string unit)
    {
        list[key] = new ShoppingListItem
        {
            Item = ingredient.Name,
            Amount = amount,
            Unit = unit
        };
    }
}