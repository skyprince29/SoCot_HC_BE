namespace SoCot_HC_BE.Utils
{
    public static class ValidationHelper
    {
        public static void AddError(Dictionary<string, List<string>> errors, string field, string message)
        {
            if (!errors.ContainsKey(field))
                errors[field] = new List<string>();

            errors[field].Add(message);
        }

        public static void IsRequired(Dictionary<string, List<string>> errors, string field, object? value, string label)
        {
            bool isMissing = value == null;

            if (value is string str)
            {
                isMissing = string.IsNullOrWhiteSpace(str);
            }
            else if (value is Guid guid)
            {
                isMissing = guid == Guid.Empty;
            }
            else if (value is int intVal)
            {
                isMissing = intVal == 0;
            }
            else if (value is long longVal)
            {
                isMissing = longVal == 0;
            }
            else if (value != null && value.GetType().IsEnum)
            {
                var underlying = Convert.ToInt32(value); // or Convert.ToByte(value) if you expect byte enums
                isMissing = underlying == 0;
            }

            if (isMissing)
            {
                AddError(errors, field, $"{label} is required.");
            }
        }
    }
}
