using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ApplicationUI.Statics;

public class ServerService
{
    //private const string BaseUrl = "https://kukumber.itstep.click";
    private const string BaseUrl = "http://localhost:5281";
    public class ImageResult
    {
        public bool IsSuccess { get; set; }
        public string ImageUrl { get; set; }
        public string ErrorMessage { get; set; }
    }

    public static async Task<ImageResult> UploadImageAsync(string imagePath)
    {
        try
        {
            using HttpClient httpClient = new HttpClient();
            string apiUrl = $"{BaseUrl}/api/galleries/upload";

            var uploadModel = new { Photo = $"data:image/jpeg;base64,{Convert.ToBase64String(File.ReadAllBytes(imagePath))}" };
            StringContent content = new StringContent(JsonSerializer.Serialize(uploadModel), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                var imageUrl = JsonDocument.Parse(responseString).RootElement.GetProperty("image").GetString();

                if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
                    imageUrl = new Uri(new Uri(BaseUrl), imageUrl).ToString();

                return new ImageResult { IsSuccess = true, ImageUrl = imageUrl, ErrorMessage = null };
            }

            return new ImageResult { IsSuccess = false, ImageUrl = null, ErrorMessage = await response.Content.ReadAsStringAsync() };
        }
        catch (Exception ex)
        {
            return new ImageResult { IsSuccess = false, ImageUrl = null, ErrorMessage = ex.Message };
        }
    }

    public async Task<ImageResult> SearchImageAsync(string imageUrl)
    {
        try
        {
            using HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(imageUrl);

            return response.IsSuccessStatusCode
                ? new ImageResult { IsSuccess = true, ImageUrl = imageUrl, ErrorMessage = null }
                : new ImageResult { IsSuccess = false, ImageUrl = null, ErrorMessage = await response.Content.ReadAsStringAsync() };
        }
        catch (Exception ex)
        {
            return new ImageResult { IsSuccess = false, ImageUrl = null, ErrorMessage = ex.Message };
        }
    }

    public static async Task<byte[]> DownloadImageBytesAsync(string imageUrl)
    {
        try
        {
            using HttpClient httpClient = new HttpClient();
            return await httpClient.GetByteArrayAsync(imageUrl);
        }
        catch (Exception)
        {
            return null;
        }
    }
}
