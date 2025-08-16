using Microsoft.AspNetCore.Mvc;
using SalesEvaluation.Backend.Services.Storage;
using SalesEvaluation.Model;
using System.Security.Claims;


namespace SalesEvaluation.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileManagerController : ControllerBase
    {
        private readonly StorageService _fileService;
        public FileManagerController( StorageService service) 
        {
            _fileService = service;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }

        /// <summary>
        /// Uploads multiple files to the configured storage service. 
        /// The storage service is determined at application startup.
        /// </summary>
        /// <param name="files">
        /// A dictionary where the key is a unique identifier for each upload 
        /// and the value is an <see cref="UploadedFiles"/> object containing the file data.
        /// </param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> indicating the result of the upload operation. 
        /// On success, it returns an HTTP 200 OK with a success message.
        /// </returns>
        /// <remarks>
        /// This endpoint expects the files to be sent in the request body as JSON. 
        /// Each entry in the dictionary represents a file or group of files to upload. 
        /// The method asynchronously uploads all provided files using the registered file service.
        /// </remarks>
        [HttpPost("Upload")]
        public async Task<IActionResult> UploadCsvFiles ([FromBody] Dictionary<string, UploadedFiles> files)
        {
            var uploadedBy = $"{HttpContext.User.FindFirstValue(ClaimTypes.GivenName)} {HttpContext.User.FindFirstValue(ClaimTypes.Surname)}";
            var userId = HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value!;
            //Get content fron the downstream API content body 
            if (files.Count > 0)
            {
                await _fileService.FileUpload(files, uploadedBy , userId);
            }

            return Ok(new { message = "Files uploaded successfully." });
        }
    }
}
