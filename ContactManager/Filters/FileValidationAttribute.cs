using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactManager.Filters
{
    public class FileValidationAttribute : ActionFilterAttribute
    {
        private readonly string[] _allowedExtensions = { ".csv" };
        private readonly long _maxFileSize = 10 * 1024 * 1024; // 10 MB

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var file = context.HttpContext.Request.Form.Files.FirstOrDefault();
            if (file == null)
            {
                context.Result = new BadRequestObjectResult("No file uploaded.");
                return;
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!_allowedExtensions.Contains(fileExtension))
            {
                context.Result = new BadRequestObjectResult("Invalid file format. Only CSV files are allowed.");
                return;
            }

            if (file.Length > _maxFileSize)
            {
                context.Result = new BadRequestObjectResult("File size exceeds the maximum allowed size (10 MB).");
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
