namespace SalesEvaluation.ConfigVariables
{
    public class FilesEnv
    {
        public  long FileSizeLimit { get; set;}

        public required List<string> AllowedFileTypes { get; set; }
    }
}
