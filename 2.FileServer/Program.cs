using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

string folderName = "uploads";
string pathFolder = Path.Combine(Directory.GetCurrentDirectory(), folderName);

Directory.CreateDirectory(pathFolder); //автоматично перевіряє існування каталогу

// Додаємо обробник для статичних файлів
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(pathFolder),
    RequestPath = "/images"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
