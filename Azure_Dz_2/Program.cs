using Azure.Storage.Blobs;
using Azure_Dz_2.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews();
string dbConnStr = builder.Configuration.GetConnectionString("PhotoBD");
builder.Services.AddDbContext<PhotoContext>(opt => opt.UseSqlServer(dbConnStr));
builder.Services.AddTransient<BlobServiceClient>(fact => {
    string connStr = builder.Configuration.GetSection("AZURE_STORAGE_CONNECTION_STRING").Value;
    //string connStr = builder.Configuration.GetSection("AzureStorageEmulator").Value;
    return new BlobServiceClient(connStr);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
