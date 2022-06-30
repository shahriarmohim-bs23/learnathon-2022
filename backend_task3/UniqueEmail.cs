using backend_task3.Services;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;

namespace backend_task3
{
    public class UniqueEmail : ValidationAttribute
    {
        protected override ValidationResult IsValid(
           object value, ValidationContext validationContext)
        {
            var _context = (Userservices)validationContext.GetService(typeof(Userservices));
            var entity = _context.Users().Find(x => x.Email == value.ToString()).FirstOrDefault();

            if (entity != null)
            {
                return new ValidationResult(GetErrorMessage(value.ToString()));
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage(string email)
        {
            return $"Email {email} is already in use.";
        }
    }
}
