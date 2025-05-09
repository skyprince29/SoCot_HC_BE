namespace SoCot_HC_BE.Helpers
{
    public static class ObjectConverterHelper
    {
        public static T ConvertToNumericValue<T>(string? input) where T : struct, IConvertible
        {
            if (!string.IsNullOrEmpty(input))
            {
                var numericString = new string(input.Where(char.IsDigit).ToArray());
                if (typeof(T) == typeof(int) && int.TryParse(numericString, out var intResult))
                {
                    return (T)(object)intResult;
                }
                else if (typeof(T) == typeof(decimal) && decimal.TryParse(numericString, out var decimalResult))
                {
                    return (T)(object)decimalResult;
                }
            }
            return default; // Fallback to default value (0 for int/decimal) if input is null, empty, or invalid
        }
    }
}
