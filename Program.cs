using System.Runtime.InteropServices;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MapleDreams.Extensions;
using MapleDreams.HtmlGenerator;
using MapleDreams.Models;


var builder = WebApplication.CreateBuilder(args);

// Add appsettings.json to configuration
builder.Services.Configure<AppSettings>(builder.Configuration);

// Add HttpContext
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Display Chinese characters
builder.Services.AddSingleton(HtmlEncoder.Create(System.Text.Unicode.UnicodeRanges.All));

// Custom ModelError in ModelState
builder.Services.AddTransient<IHtmlGenerator, AlertHtmlGenerator>();

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenAnyIP(8500);
//    //options.ListenAnyIP(8301, configure => configure.UseHttps());
//});


var app = builder.Build();

// Create the SQLite database and its tables when they do not exist yet
builder.Configuration.GetConnectionString("MapleDreamsConnection")
    .EnsureSqliteDatabaseCreated(app.Logger);

app.UseExceptionHandler(new ExceptionHandlerOptions()
{
    ExceptionHandler = async context =>
        await Task.Run(() =>
        {
            var ex = context.Features.Get<IExceptionHandlerFeature>();
            if (ex is not null)
            {
                string message = $"[Message] {ex.Error.Message}{Environment.NewLine}[StackTrace] {ex.Error.StackTrace.TrimStart(' ')}";
                System.Text.Encoding.Default.GetBytes(message).SaveToFile(
                    filename: $"{DateTime.Now.ToString("yyyyMMdd_HHmmss")}_{context.TraceIdentifier}.log",
                    saveDir: @"logs/",
                    trueDir: $@"{app.Environment.WebRootPath}/");
                context.Response.Redirect("/Error");
            }
        })
});


//// Select DbConnectionStrings to use
//if (app.Environment.IsDevelopment())
//{
//    builder.Configuration["ConnectionStrings:MessageBoardConnection"] = builder.Configuration["ConnectionStrings:MessageBoardLocalConnection"];
//}

// Enable `Reverse Proxy` mode when running on Linux
if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });
}

// Use static files
var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
provider.Mappings[".properties"] = "text/plain";
app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = provider });

// Use mvc
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapRazorPages());

app.Run();
