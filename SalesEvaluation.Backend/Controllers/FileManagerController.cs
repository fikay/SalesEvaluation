using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using SalesEvaluation.Backend.Services.Storage;
using SalesEvaluation.Model;

namespace SalesEvaluation.Backend.Controllers
{
    public class FileManagerController : Controller
    {
        private readonly ILogger<FileManagerController> _logger;
        private readonly StorageService _fileService;
        public FileManagerController( StorageService service) 
        {
            _fileService = service;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost(Name ="Upload")]
        public async Task<IActionResult> UploadCsvFiles ([FromBody] Dictionary<string, UploadedFiles> files)
        {
            //Get content fron the downstream API content body 
            if (files.Count > 0)
            {
                await _fileService.FileUpload(files);
            }
        }
    }
}
