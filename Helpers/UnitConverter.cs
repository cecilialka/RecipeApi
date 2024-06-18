namespace RecipeApi.Helpers;

public static class UnitConverter
{
    private static readonly Dictionary<string, double> UnitConversionFactors = new Dictionary<string, double>
    {
        { "g", 1.0 },
        { "kg", 1000.0 },
        { "ml", 1.0 },
        { "l", 1000.0 }
    };

    public static double Convert(double amount, string fromUnit, string toUnit)
    {
        if (fromUnit == toUnit)
        {
            return amount;
        }

        if (UnitConversionFactors.ContainsKey(fromUnit) && UnitConversionFactors.ContainsKey(toUnit))
        {
            double fromFactor = UnitConversionFactors[fromUnit];
            double toFactor = UnitConversionFactors[toUnit];
            return amount * (fromFactor / toFactor);
        }

        throw new InvalidOperationException($"Cannot convert from {fromUnit} to {toUnit}");
    }
}