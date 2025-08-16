namespace SalesEvaluation.Shared.Models
{
    public class StorageEnums
    {
        public enum StorageType
        {
            Blob,
            Sql
        }
        public enum FileType
        {
            Csv,
            Json,
            Xml,
            Txt,
            Other
        }
        public enum FileStatus
        {
            Pending,
            Processing,
            Completed,
            Failed
        }
    }
}
