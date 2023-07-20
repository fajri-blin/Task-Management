using System.ComponentModel.DataAnnotations;

namespace Task_Management.Utilities.Enum
{
    public class ConfirmPasswordAttribute : ValidationAttribute
    {
        private readonly string _password;
        public ConfirmPasswordAttribute(string password)
        {
            _password = password;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var passwordProperty = validationContext.ObjectType.GetProperty(_password);

            if (passwordProperty != null)
            {
                var passwordValue = passwordProperty.GetValue(validationContext.ObjectInstance, null);

                if (value != null && passwordValue != null && !value.Equals(passwordValue))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
}
