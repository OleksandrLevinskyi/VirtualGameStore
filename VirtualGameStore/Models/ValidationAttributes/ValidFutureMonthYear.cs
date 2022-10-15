using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace VirtualGameStore.Models.ValidationAttributes;

public class ValidFutureMonthYear: ValidationAttribute
{
    private static readonly Regex Pattern = new Regex("^(0[1-9]|1[012])/\\d{2}$");
    private static readonly (int, int) DateComponents = (DateTime.Now.Month, DateTime.Now.Year % 100);
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Got to have some value first of all
        if (value is not string str)
            return new ValidationResult("A expiration date is required.");
        // Make sure it matches our regex
        if (!Pattern.IsMatch(str))
            return new ValidationResult("Date must be a month and year in the format '00/00'.");
        // Make sure it is either this month or further in the future. Also, because of the regex,
        // we KNOW that we will have two valid integers seperated by a '/'. Nothing more, nothing less.
        int[] dc = Array.ConvertAll(str.Split("/"), int.Parse);
        if (dc[1] < DateComponents.Item2 || (dc[1] == DateComponents.Item2 && dc[0] < DateComponents.Item1))
            return new ValidationResult("Date must not be in the past");
        // That's all the cases covered. If we've gotten here, its valid.
        return ValidationResult.Success;
    }
}