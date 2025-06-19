using frontend_razor.Services;

var builder = WebApplication.CreateBuilder(args);

// Ajouter les services
builder.Services.AddHttpsRedirection(options => {
    options.HttpsPort = 5093; // ou votre port HTTPS
});
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IDetectionService, DetectionService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();