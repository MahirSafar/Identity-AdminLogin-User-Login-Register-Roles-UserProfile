using System.ComponentModel.DataAnnotations;

namespace Pustok.App.Attributes
{
    public class FileLengthAttribute : ValidationAttribute
    {
        private readonly int _maxLength;
        public FileLengthAttribute(int maxLength)
        {
            _maxLength = maxLength*1024*1024;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            var files = GetFilesToValidate(value);  

            foreach (var item in files)
            {
                if (item?.Length > _maxLength)
                {
                    return new ValidationResult($"File size must be less than {_maxLength} MB.");
                }
            }
            
            return ValidationResult.Success;
        }
        private IEnumerable<IFormFile> GetFilesToValidate(object value)
        {
            return value switch
            {
                IFormFile file => new[] { file },
                IEnumerable<IFormFile> files => files,
                _ => Enumerable.Empty<IFormFile>()
            };
        }
    }
}
