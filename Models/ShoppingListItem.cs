namespace RecipeApi.Models;

public class ShoppingListItem {
    public required string Item { get; set; }
    public double Amount { get; set; }
    public required string Unit { get; set; }
}