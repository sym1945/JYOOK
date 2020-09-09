namespace JYOOK.Application
{
    public static class StringExtensions
    {
        public static bool IsEmptyOrNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }
    }
}