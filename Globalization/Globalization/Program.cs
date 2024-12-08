using Globalization.Controllers;  // Add your controllers namespace
using Microsoft.Extensions.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure CORS to allow specific origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder => builder
            .WithOrigins("http://localhost:4200", "https://yourfrontendurl.com") // Add allowed origins here
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// Add controllers
builder.Services.AddControllers();

// Add Swagger for API documentation
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure Serilog for logging
Log.Information($"API Started. Start Time: {DateTime.Now}");

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // The default HSTS value is 30 days.
}

// Enable HTTPS redirection
app.UseHttpsRedirection();
app.UseStaticFiles();

// Enable CORS middleware before other middleware
app.UseCors("AllowSpecificOrigins");

// Use routing
app.UseRouting();

// Enable Swagger middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Globalization API v1");
    c.RoutePrefix = string.Empty;  // Set Swagger UI at the root URL
});

// Map controllers (e.g., MasterController)
app.MapControllers();  // This will map your `MasterController` and other controllers

// Run the application
app.Run();
