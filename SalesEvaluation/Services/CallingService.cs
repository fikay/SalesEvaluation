using Microsoft.Identity.Abstractions;
using SalesEvaluation.Model;
using System.Text;
using System.Text.Json;

namespace SalesEvaluation.Services
{
    public class CallingService
    {
        private IDownstreamApi _downstreamApi;

        public CallingService(IDownstreamApi downstreamApi)
        {
            _downstreamApi = downstreamApi;
        }


        public async Task<string> GetWeather()
        {
            var result = await _downstreamApi.CallApiForUserAsync("SalesEvalAPI", opt =>
            {
                opt.RelativePath = "WeatherForecast";
                opt.HttpMethod = HttpMethod.Get.ToString();
            });
            
            var response = await result.Content.ReadAsStringAsync();
            return response;
            //if (result.IsSuccessStatusCode)
            //{
            //    return response;
            //}
            //else
            //{
            //    throw new Exception($"Error calling API: {response}");
            //}
        }

        public async Task ProcessUploadedFilesAsync(Dictionary<string, UploadedFiles> files)
        {
            // Convert the dictionary to a JSON string
            var content = JsonContent.Create(files);

            var result = await _downstreamApi.CallApiForUserAsync("SalesEvalAPI", opt =>
            {
                opt.RelativePath = "FileManager/Upload";
                opt.HttpMethod = HttpMethod.Post.Method;
            }, content: content);
        }
    }
}
