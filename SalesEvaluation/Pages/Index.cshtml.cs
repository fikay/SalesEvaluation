using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using SalesEvaluation.ConfigVariables;
using SalesEvaluation.Model;
using SalesEvaluation.Services;
using SalesEvaluation.Utilities;

namespace SalesEvaluation.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly CallingService _svc;
    private readonly FilesEnv _filesConfiguration;

    [BindProperty]
    public MultipleFileUpload MultipleFileUpload { get; set; } = new MultipleFileUpload();

    public IndexModel(ILogger<IndexModel> logger, CallingService callingService, IOptions<FilesEnv> configuration)
    {
        _logger = logger;
        _svc = callingService;
        _filesConfiguration = configuration.Value;
    }

    public async Task  OnGet()
    {
       var result = await _svc.GetWeather();
    }

    public async Task<IActionResult> OnPostFileUploadAsync( CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid || MultipleFileUpload.files.Count < 1)
        {
            return Page();
        }

        //Pre-processing the uploaded files before sending over to the server
        var formFileContent = await FileHelpers.ProcessFormFile(MultipleFileUpload.files ,_filesConfiguration );

        //Take  files qith no errors and send them to the server
        var filesToUpload = formFileContent.Where(f => string.IsNullOrEmpty(f.Value.ErrorMessage)).ToDictionary(kvp => kvp.Key , kvp => kvp.Value);
        if (filesToUpload.Count == 0)
        {
            ModelState.AddModelError("files", "No valid files to upload.");
            return Page();
        }

        //Call the API to upload files
         await _svc.ProcessUploadedFilesAsync(filesToUpload);
        return Page();
    }


    public void onPostCancelUpload()
    {
        //Use cancellation token to cancel the upload

    }
}
  