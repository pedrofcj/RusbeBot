namespace RusbeBot.Core.Extensions;

public static class StringExtension
{
    public static TimeSpan? ToTimeSpan(this string text)
    {
        if (string.IsNullOrEmpty(text)) return null;
        
        // extract just numbers from text
        var numbers = new string(text.Where(char.IsDigit).ToArray());

        if (numbers.Length == 0) return null;
        
        // extract unit from text
        var response = text.ToLower().Last() switch
        {
            's' => TimeSpan.FromSeconds(int.Parse(numbers)),
            'm' => TimeSpan.FromMinutes(int.Parse(numbers)),
            'h' => TimeSpan.FromHours(int.Parse(numbers)),
            'd' => TimeSpan.FromDays(int.Parse(numbers)),
            _ => TimeSpan.FromSeconds(int.Parse(numbers))
        };

        return response;
    }
}