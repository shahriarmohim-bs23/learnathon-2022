using backend_task3.Services;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;

namespace backend_task3
{
    public class UniqueName : ValidationAttribute
    {
        protected override ValidationResult IsValid(
           object value, ValidationContext validationContext)
        {
            var _context = (Userservices)validationContext.GetService(typeof(Userservices));
            var entity = _context.Users().Find(x => x.Username == value.ToString()).FirstOrDefault();

            if (entity != null)
            {
                return new ValidationResult(GetErrorMessage(value.ToString()));
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage(string name)
        {
            return $"Name {name} is already in use.";
        }
    }
}
