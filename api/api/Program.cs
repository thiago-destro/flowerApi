using api;

var builder = WebApplication.CreateBuilder(args);

//app.MapGet("/", () => "Hello World!");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapControllers();

var imagesPath = builder.Configuration.GetSection("FilesPath").Value ??
                 Path.Combine(builder.Environment.WebRootPath, "Images");
FlowersHelper.LoadFlowers(imagesPath);

app.Run();