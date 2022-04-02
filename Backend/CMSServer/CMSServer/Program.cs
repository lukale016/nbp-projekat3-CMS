using CMSServer.Services.FileSystemManager;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//CORS
builder.Services.AddCors();
//Configuration
builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("MongoConfig"));
//Singleton register
builder.Services.AddSingleton<IFileSystemManagerService, FileSystemManagerService>();
builder.Services.AddSingleton<UnitOfWork>();

if (!Directory.Exists("UserData"))
{
    Directory.CreateDirectory("UserData");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options =>
{
    options.AllowAnyHeader()
           .AllowAnyMethod()
           .AllowAnyOrigin();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
