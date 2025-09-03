using System.ComponentModel.DataAnnotations;

namespace Pustok.App.Attributes
{
    public class FileTypeAttribute : ValidationAttribute
    {
        private readonly string[] _allowedTypes;
        public FileTypeAttribute(params string[] allowedTypes)
        {
            _allowedTypes = allowedTypes;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var list = new List<IFormFile>();
            var file = value as IFormFile;
            if (file != null)
            {
                list.Add(file);
            }
            var files = value as List<IFormFile>;   
            if (files != null)
            {
                list.AddRange(files);
            }
            foreach (var item in list)
            {
                if (!_allowedTypes.Contains(item.ContentType))
                {
                    return new ValidationResult($"File type must be one of the following: {string.Join(", ", _allowedTypes)}.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
