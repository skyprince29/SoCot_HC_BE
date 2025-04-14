namespace SoCot_HC_BE.Utils
{
    public class ModelValidationException : Exception
    {
        public Dictionary<string, List<string>> Errors { get; }

        public ModelValidationException(string message, Dictionary<string, List<string>> errors) : base(message)
        {
            Errors = errors;
        }

        public ModelValidationException(string message) : base(message)
        {
            Errors = new Dictionary<string, List<string>>();
        }
    }
}
