using Scalar.AspNetCore;
using BookHistoryService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

 builder.Services.AddSingleton<IBookService, ScienceBookService>();
//builder.Services.AddSingleton<IBookService, HistoryBookService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors("ReactApp");
app.UseAuthorization();

app.MapControllers();

app.Run();
