using System.ComponentModel.DataAnnotations;

namespace ShrinkLinkApp.Attributes
{
    public class DateAttribute : ValidationAttribute
    {
        //public DateAttribute()
        //  : base(typeof(DateTime),
        //         DateTime.Now.ToString(),
        //         DateTime.Now.AddYears(1).ToString())

        //{ }
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) 
                return new ValidationResult("The expiration date of a short URL is no more than 1 year");

            var date  = (DateTime)value;
            if (date <= DateTime.Now.AddYears(1))
            {
                return ValidationResult.Success!;
            }
            else
            {
                return new ValidationResult("The expiration date of a short URL is no more than 1 year");
            }
        }
    }
}