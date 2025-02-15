using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace _2.FileServer.Controllers
{
    public class UploadImage
    {
        /// <summary>
        /// Це буде зображення у форматі base64 прилітати на сервак
        /// </summary>
        public string Photo { get; set; } = String.Empty;
    }

    [Route("api/[controller]")]
    [ApiController]
    public class GalleriesController : ControllerBase
    {
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadImage([FromBody]UploadImage model)
        {
            try
            {
                string fileName = $"{Guid.NewGuid()}.jpg";
                if(model.Photo.Contains(','))
                    model.Photo = model.Photo.Split(',')[1];
                byte[] byteArray = Convert.FromBase64String(model.Photo);

                // Читаємо байти як зображення
                using Image image = Image.Load(byteArray);
                
                // Масштабуємо зображення до 200x200
                image.Mutate(x => x.Resize(
                    new ResizeOptions
                    {
                        Size = new Size(600, 600), // Максимальний розмір
                        Mode = ResizeMode.Max // Зберігає пропорції без обрізання
                    }
                    ));

                string folderName = "uploads";
                string pathFolder = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                // Шлях до збереження файлу
                string outputFilePath = Path.Combine(pathFolder, fileName);

                // Зберігаємо файл у вказаному форматі
                image.Save(outputFilePath);
                return Ok(new {image = $"/images/{fileName}" });

            }
            catch(Exception ex) 
            {
                return BadRequest(new { error = ex.Message });
            }
            
        }
    }
}
