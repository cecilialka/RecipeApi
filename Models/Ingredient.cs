namespace RecipeApi.Models;

public class Ingredient {
    public int Id {get; set;}
    public double Amount {get; set;}
    public required string Consistency {get; set;}
    public List<string> Meta {get; set;} = new();
    public required string Name {get; set;}
    public required string Original {get; set;}
    public string Unit {get; set;} = string.Empty;
    public required Measure Measures {get; set;}
}