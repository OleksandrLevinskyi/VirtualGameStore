using System.ComponentModel.DataAnnotations;

namespace VirtualGameStore.ValidationAttributes
{
    public class DateNotInTheFutureAttribute : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return $"{name} should not be in the future.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var dateValue = value as DateTime?;

            if (dateValue == null || dateValue > DateTime.Now.Date)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}
