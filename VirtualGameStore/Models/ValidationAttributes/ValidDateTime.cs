using System.ComponentModel.DataAnnotations;

namespace VirtualGameStore.Models.ValidationAttributes
{
    public class ValidDateTime : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (DateTime.Now < (DateTime)value)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Event date/time must be in the future.");
        }
    }
}
