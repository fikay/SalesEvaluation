using Microsoft.Data.SqlClient;

namespace SalesEvaluation.Backend.Utils.DatabaseInitializer
{
    public static class DbInitializer
    {
        public static async Task RunScriptsAsync( string connectionString)
        {
            //Get sql Files from the SqlScripts folder
            var sqlFiles = Directory.GetFiles("SqlScripts", "*.sql");

            if (sqlFiles.Length == 0)
            {
                throw new FileNotFoundException("No SQL scripts found in the SqlScripts folder.");
            }

            //Separation of Concerns 
            var proc = sqlFiles.Where(x => x.Contains("Procedure")).ToList();
            var creation = sqlFiles.Where(x => x.Contains("Creation"));

            if (sqlFiles.Length > 0 )
            {
                foreach (var script  in  sqlFiles)
                {
                    //Read the file 
                    var content = await File.ReadAllTextAsync(script);
                    content = content.Trim(new char[] { '\r', '\n' });

                    List<Task> section = new List<Task>();

                    //Break all int sections based on the Go directive
                    var scriptSections = content.Split(new[] { "\r\nGO", "\nGO", "GO" }, StringSplitOptions.RemoveEmptyEntries);
                   

                    foreach (var sec in scriptSections)
                    {
                        section.Add(RunScripts(sec, connectionString));
                    }

                    //Run all sections in parallel
                    await Task.WhenAll(section);

                }
            }
        }


        private static async Task RunScripts(string script, string connection)
        {
            try
            {
                using (var connect = new SqlConnection(connection))
                {
                    connect.Open();
                    var command = new SqlCommand(script, connect);
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to run SQL script.", ex);
            }   
           
        }

    }
}
