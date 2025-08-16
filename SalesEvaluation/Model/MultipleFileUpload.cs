using System.ComponentModel.DataAnnotations;

namespace SalesEvaluation.Model
{
    public class MultipleFileUpload
    {
        [Required]
        [Display(Name = "files")]
        public List<IFormFile> files { get; set; } = new();
    }
}
